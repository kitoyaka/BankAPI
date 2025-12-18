using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.API.AddControllers
{
    [Route("api/[controller]")]
    [ApiController]



    
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransactionController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto request)
        {
            var senderAccount = await _context.Accounts.FindAsync(request.SenderAccountId);
            var receiverAccount = await _context.Accounts.FindAsync(request.ReceiverAccountId);
            if(senderAccount == null || receiverAccount == null)
            {
                return NotFound("One or both accounts not found.");
            }
            if(request.Amount <= 0)
            {
                return BadRequest("Transfer amount must be greater than zero.");
            }
            if(senderAccount.Balance < request.Amount)
            {
                return BadRequest("Insufficient funds in sender's account.");
            }
            senderAccount.Balance -= request.Amount;
            receiverAccount.Balance += request.Amount;
            _context.Accounts.Update(senderAccount);
            _context.Accounts.Update(receiverAccount);
            _context.Transactions.Add(new Transaction
            {
                SenderAccountId = senderAccount.Id,
                ReceiverAccountId = receiverAccount.Id,
                Amount = request.Amount,
                TransactionDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();  

            return Ok();
        }

        [HttpGet("transactions/{accountId}")] 

        public async Task<IActionResult> GetTransationsForAccount(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if(account == null)
            {
                return NotFound("Account not found.");
            }
            var transaction = await _context.Transactions
            .Where(t => t.SenderAccountId == accountId || t.ReceiverAccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => new TransactionDto
            {
                transactionId = t.Id,
                Amount = t.Amount,
                Date = t.TransactionDate,
                SenderAccountId = t.SenderAccountId,
                ReceiverAccountId = t.ReceiverAccountId
            })
            .ToListAsync();

            return Ok(transaction);
        }


    }


}