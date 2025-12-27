
using Bank.Core.Entities;

namespace Bank.Infrastructure.Extensions
{
    public static class AccountExtensions
    {
        public static bool HasEnoughBalance(this Account account, decimal amount)
        {
            return account.Balance >= amount;
        }
        
            
    

    }
}