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
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
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
    }
}