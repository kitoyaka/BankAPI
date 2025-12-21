using Bank.Core.DTOs;

namespace Bank.Core.Interfaces 
{
    public interface IUserService 
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task DeleteUserAsync(int userId);
    }
}