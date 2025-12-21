
using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Bank.Core.Interfaces;


namespace Bank.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
          if(await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new Exception("Email is already in use.");
            }

            var user = new User
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Password = registerDto.Password,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
    

}