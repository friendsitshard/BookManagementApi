using BookManagmentApi.DAL.Interfaces;
using BookManagmentApi.DAL.Services;
using BookManagmentApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _authService.RegisterAsync(userDto);
                return Ok(new { message = "User registered successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message }); 
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(userDto);

            if (token == null)
                return Unauthorized(new { error = "Invalid email or password" });

            return Ok(new { token });
        }
    }
}

