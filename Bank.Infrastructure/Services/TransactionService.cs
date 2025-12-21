using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Core.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task TransferAsync(TransferDto request)
        {
            
            var senderAccount = await _context.Accounts.FindAsync(request.SenderAccountId);
            var receiverAccount = await _context.Accounts.FindAsync(request.ReceiverAccountId);

            if (senderAccount == null || receiverAccount == null)
            {
                throw new Exception("One or both accounts not found.");
            }
            if (request.Amount <= 0)
            {
                throw new Exception("Transfer amount must be greater than zero.");
            }
            if (senderAccount.Balance < request.Amount)
            {
                throw new Exception("Insufficient funds in sender's account.");
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
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsForAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            var transactions = await _context.Transactions
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

            return transactions;
        }
    }
}