using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Code.Generator.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly CodeGenDbContext _context;

        public AuthService(CodeGenDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),              
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password
            };

            var result = await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return GenerateJwtToken(user);

            //throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(i => i.Email == loginDto.Email && i.PasswordHash == loginDto.Password);
            if (user != null)
            {
                return GenerateJwtToken(user);
            }

            throw new UnauthorizedAccessException("Invalid credentials");
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
