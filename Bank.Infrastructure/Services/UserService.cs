using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Bank.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims; 
using System.Text; 
using Microsoft.IdentityModel.Tokens; 
using Microsoft.Extensions.Configuration; 

namespace Bank.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new Exception("Email is already in use.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            string userRole = "Client";
            if(registerDto.AdminSecretKey == "Super")
            {
                userRole = "Admin";
            }
            var user = new User
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Password = passwordHash,
                Role = userRole,
                CreatedAt = DateTime.UtcNow
            };

         
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

         
            var fakeIBAN = "UA" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 27).ToUpper();
            
            var account = new Account
            {
                UserId = user.Id,
                currency = "UAH",
                Balance = 0,
                IBAN = fakeIBAN
            };

           
            _context.Accounts.Add(account); 
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {   
    
        var user = await _context.Users
        .Include(u => u.Account)
        .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
        throw new Exception("User not found.");
        }


        user.IsDeleted = true;
    
        if (user.Account != null)
        {
        user.Account.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
        }


        public async Task<string> LoginUserAsync(string email, string password)
        {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new Exception("Invalid email or password.");
        }

        return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var key = _configuration["JwtSettings:Key"] ?? throw new Exception("JWT Key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: Claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}