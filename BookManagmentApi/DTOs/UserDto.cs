using System.ComponentModel.DataAnnotations;

namespace BookManagmentApi.DTOs
{
    public class UserDto
    {
        [EmailAddress]
        public required string Email { get; init; }
        public required string Password {  get; init; }
    }
}
