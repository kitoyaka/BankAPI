using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Core.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAccountAsync(CreateAccountDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var fakeIBAN = "UA" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 27).ToUpper();

            var newAccount = new Account
            {
                UserId = request.UserId,
                currency = request.Currency,
                Balance = 0,
                IBAN = fakeIBAN
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> DepositAsync(DepositDto request)
        {
            var account = await _context.Accounts.FindAsync(request.AccountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }
            if (request.Amount <= 0)
            {
                throw new Exception("Deposit amount must be greater than zero");
            }

            account.Balance += request.Amount;
            await _context.SaveChangesAsync();
            return account.Balance;
        }

        public async Task<List<Account>> GetUserByIdAsync(int userId)
        {
            
            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();
                
            return accounts;
        }
    } 
}