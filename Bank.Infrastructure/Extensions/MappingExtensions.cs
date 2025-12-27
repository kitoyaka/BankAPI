
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
                currency = "UAH",
                Balance = 0,
                IBAN = fakeIBAN
            };
        }


    }


}