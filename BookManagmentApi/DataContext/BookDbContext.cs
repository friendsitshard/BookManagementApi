using BookManagmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagmentApi.DataContext
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasIndex(b => new { b.IsDeleted, b.ViewsCount, b.PublicationYear })
                .HasDatabaseName("IX_Books_IsDeleted_ViewsCount_PublicationYear");
        }
    }
}
