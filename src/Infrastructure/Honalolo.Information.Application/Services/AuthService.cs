using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Honalolo.Information.Application.DTOs.Users;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Honalolo.Information.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly TouristInfoDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(TouristInfoDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // 1. Check if email exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("User with this email already exists.");

            // 2. Hash Password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 3. Create User
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = passwordHash
            };

            var userObj = _context.Users.Add(user);

            User returnUserData = new()
            {
                Id = userObj.Entity.Id,
                UserName = userObj.Entity.UserName,
                Email = userObj.Entity.Email,
                Role = userObj.Entity.Role,
                PasswordHash = userObj.Entity.PasswordHash
            };

            await _context.SaveChangesAsync();

            // 4. Login immediately (Generate Token)
            return GenerateToken(returnUserData);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Find User
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("Invalid credentials.");

            // 2. Verify Password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            User returnUserData = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                PasswordHash = user.PasswordHash
            };

            // 3. Generate Token
            return GenerateToken(user);
        }

        private AuthResponseDto GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // CRITICAL: This saves "Administrator" or "Moderator" into the token
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:DurationInMinutes"]!)),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }
    }
}