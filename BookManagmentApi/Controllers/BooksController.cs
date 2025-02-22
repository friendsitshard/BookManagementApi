using BookManagmentApi.DAL.Interfaces;
using BookManagmentApi.DAL.Services;
using BookManagmentApi.DTOs;
using BookManagmentApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagmentApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBook _booksService;

        public BooksController(IBook booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                var books = await _booksService.GetBooksAsync(pageNumber, pageSize);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving books.", error = ex.Message });
            }
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> GetBook(string title)
        {
            try
            {
                var book = await _booksService.GetBookAsync(title);
                if (book == null)
                    return NotFound(new { message = "Book not found" });

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the book.", error = ex.Message });
            }
        }

        [HttpPost("add-single")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            try
            {
                await _booksService.AddBookAsync(book);
                return CreatedAtAction(nameof(GetBook), new { title = book.Title }, book);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the book.", error = ex.Message });
            }
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddBooks([FromBody] IEnumerable<Book> books)
        {
            try
            {
                await _booksService.AddBooksAsync(books);
                return Ok(new { message = "Books added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding books.", error = ex.Message });
            }
        }

        [HttpPut("{title}")]
        public async Task<IActionResult> UpdateBook(string title, [FromBody] Book book)
        {
            try
            {
                await _booksService.UpdateBookAsync(title, book);
                return Ok(new { message = "Book updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the book.", error = ex.Message });
            }
        }

        [HttpDelete("{title}")]
        public async Task<IActionResult> SoftDeleteBook(string title)
        {
            try
            {
                await _booksService.DeleteBookAsync(title);
                return Ok(new { message = "Book deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the book.", error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> SoftDeleteBooks([FromBody] IEnumerable<string> titles)
        {
            try
            {
                await _booksService.DeleteBooksAsync(titles);
                return Ok(new { message = "Books deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting books.", error = ex.Message });
            }
        }
    }
}
