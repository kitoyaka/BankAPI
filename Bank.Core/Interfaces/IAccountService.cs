
using Bank.Core.DTOs;
using Bank.Core.Entities;

namespace Bank.Core.Interfaces
{
    public interface IAccountService
    {
        public Task CreateAccountAsync(CreateAccountDto request);
        public Task<decimal> DepositAsync(DepositDto request);
        public Task<List<Account>> GetUserByIdAsync(int userId);
    }
}