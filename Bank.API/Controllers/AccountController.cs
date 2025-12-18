using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.API.AddControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount(CreateAccountDto request)
        {
            
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
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

            return Ok(new {Message = "Account created successfully", AccountId = newAccount.Id, IBAN = newAccount.IBAN });
        }


        [HttpGet("my-accounts/{userId}")]
        public async Task<IActionResult> GetUserAccounts(int userId)
        {
            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    a.Id,
                    a.IBAN,
                    a.Balance,
                    a.currency
                })
                .ToListAsync();
                return Ok(accounts);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDto request)
        {
            var Account = await _context.Accounts.FindAsync(request.AccountId);
            if (Account == null)
            {
                return NotFound("Account not found.");
            }
            if(request.Amount <= 0)
            {
                return BadRequest("Deposit amount must be greater than zero.");
            }

            Account.Balance += request.Amount;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Deposit successful", NewBalance = Account.Balance });
        }
    }
}