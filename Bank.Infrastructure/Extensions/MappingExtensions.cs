
using Bank.Core.Entities;
using Bank.Core.DTOs;


namespace Bank.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        public static User ToUserEntity(this RegisterDto registerDto, string passwordHash, string role)
        {
            return new User
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Password = passwordHash,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static Account CreateDefaultAccount(this User user)
        {
            var fakeIBAN = "UA" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 27).ToUpper();

            return new Account
            {
                UserId = user.Id,
                Currency = "UAH",
                Balance = 0,
                IBAN = fakeIBAN
            };
        }

        public static Transaction ToTransactionEntity(this TransferDto transferDto)
        {
            return new Transaction
            {
                SenderAccountId = transferDto.SenderAccountId,
                ReceiverAccountId = transferDto.ReceiverAccountId,
                Amount = transferDto.Amount,
                TransactionDate = DateTime.UtcNow
            };
        }

        public static TransactionDto ToTransactionDto(this Transaction transaction)
        {
            return new TransactionDto
            {
                TransactionId = transaction.Id,
                SenderAccountId = transaction.SenderAccountId,
                ReceiverAccountId = transaction.ReceiverAccountId,
                Amount = transaction.Amount,
                Date = transaction.TransactionDate
            };
        }


    }


}