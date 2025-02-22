using BookManagmentApi.DAL.Interfaces;
using BookManagmentApi.DataContext;
using BookManagmentApi.DTOs;
using BookManagmentApi.Helpers;
using BookManagmentApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManagmentApi.DAL.Services
{
    public class AuthService : IAuth
    {
        private readonly JwtSettings _jwtSettings;
        private readonly BookDbContext _context;
        public AuthService(IOptions<JwtSettings> jwtSettings, BookDbContext context)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }

        public async Task RegisterAsync(UserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("User with this email already exists");

            if (userDto.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                Email = userDto.Email,
                Password = passwordHash
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task<string?> LoginAsync(UserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                return null;

            return GenerateJwtToken(user.Id);
        }

        public string GenerateJwtToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userId)),
                ]),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
