using BookManagmentApi.DTOs;

namespace BookManagmentApi.DAL.Interfaces
{
    public interface IAuth
    {
        Task RegisterAsync(UserDto user);
        Task<string?> LoginAsync(UserDto user);
    }
}
