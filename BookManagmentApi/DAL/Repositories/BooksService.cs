using BookManagmentApi.DAL.Interfaces;
using BookManagmentApi.DataContext;
using BookManagmentApi.DTOs;
using BookManagmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagmentApi.DAL.Services
{
    public class BooksService : IBook
    {
        private readonly BookDbContext _context;

        public BooksService(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetBooksAsync(int pageNumber, int pageSize)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => !b.IsDeleted)  
                .OrderByDescending(b => (b.ViewsCount * 0.5) + ((DateTime.UtcNow.Year - b.PublicationYear) * 2))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => b.Title)
                .ToListAsync();
        }

        public async Task<BookDto?> GetBookAsync(string title)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Title == title && !b.IsDeleted);
                if (book == null) return null;

                book.ViewsCount++;
                await _context.SaveChangesAsync(); 

                await transaction.CommitAsync(); 

                int yearsSincePublished = DateTime.UtcNow.Year - book.PublicationYear;
                double popularityScore = (book.ViewsCount * 0.5) + (yearsSincePublished * 2);

                return new BookDto
                {
                    Title = book.Title,
                    AuthorName = book.AuthorName,
                    PublicationYear = book.PublicationYear,
                    ViewsCount = book.ViewsCount,
                    PopularityScore = popularityScore
                };
            }
            catch
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }

        public async Task AddBookAsync(Book book)
        {
            if (await _context.Books.AnyAsync(b => b.Title == book.Title))
                throw new Exception("A book with this title already exists.");

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task AddBooksAsync(IEnumerable<Book> books)
        {
            var existingBooks = await _context.Books.Select(b => b.Title).ToListAsync();
            var duplicateTitles = books.Where(b => existingBooks.Contains(b.Title)).Select(b => b.Title).ToList();
            var newBooks = books.Where(b => !existingBooks.Contains(b.Title)).ToList();

            if (duplicateTitles.Count != 0)
            {
                throw new Exception($"The following books already exist: {string.Join(", ", duplicateTitles)}");
            }

            _context.Books.AddRange(newBooks);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(string title, Book book)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Title == title);
            if (existingBook == null || existingBook.IsDeleted)
                throw new Exception("Book not found.");

            existingBook.Title = book.Title;
            existingBook.AuthorName = book.AuthorName;
            existingBook.PublicationYear = book.PublicationYear;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(string title)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Title == title);
            if (book == null)
                throw new Exception("Book not found.");

            book.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBooksAsync(IEnumerable<string> titles)
        {
            var books = await _context.Books.Where(b => titles.Contains(b.Title)).ToListAsync();
            if (books.Count == 0)
                throw new Exception("No matching books found.");

            foreach (var book in books)
                book.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
    }
}

