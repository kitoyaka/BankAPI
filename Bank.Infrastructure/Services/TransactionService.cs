using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Core.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Bank.Infrastructure.Extensions;

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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
            

            var senderAccount = await _context.Accounts.FindAsync(request.SenderAccountId);
            var receiverAccount = await _context.Accounts.FindAsync(request.ReceiverAccountId);

            if (senderAccount == null || receiverAccount == null)
            {
                throw new Exception("Sender account not found.");
            }

            senderAccount.Debit(request.Amount);
            receiverAccount.Credit(request.Amount);

    
            _context.Accounts.Update(senderAccount);
            _context.Accounts.Update(receiverAccount);

            var transactionEntity = request.ToTransactionEntity();
            _context.Transactions.Add(transactionEntity);
            
            

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            } 
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
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
                .Select(t => t.ToTransactionDto())
                .ToListAsync();

            return transactions;
        }
    }
}