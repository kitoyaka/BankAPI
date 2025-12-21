
using Bank.Core.DTOs;

namespace Bank.Core.Interfaces
{

    public interface ITransactionService
    {
        public Task TransferAsync(TransferDto request);
        public Task<IEnumerable<TransactionDto>> GetTransactionsForAccountAsync(int accountId);

    }

}