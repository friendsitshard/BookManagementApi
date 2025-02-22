using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookManagmentApi.Models
{
    public class Book
    {
        [Key]
        [MinLength(1)]
        [Required]
        public string Title { get; set; } = string.Empty;
        [Range(1, 9999, ErrorMessage = "Invalid publication year")]
        public required short PublicationYear { get; set; }
        [MinLength(1)]
        [Required]
        public string AuthorName { get; set; } = string.Empty;
        public int ViewsCount { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
    }
}
