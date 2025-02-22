using BookManagmentApi.DTOs;
using BookManagmentApi.Models;

namespace BookManagmentApi.DAL.Interfaces
{
    public interface IBook
    {
        Task<IEnumerable<string>> GetBooksAsync(int pageNumber, int pageSize);
        Task<BookDto?> GetBookAsync(string title);
        Task AddBookAsync(Book book);
        Task AddBooksAsync(IEnumerable<Book> books);
        Task UpdateBookAsync(string title, Book book);
        Task DeleteBookAsync(string title);
        Task DeleteBooksAsync(IEnumerable<string> titles);
    }
}
