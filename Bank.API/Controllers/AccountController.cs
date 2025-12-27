using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bank.Core.Interfaces;

namespace Bank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount(CreateAccountDto request)
        {
            await _accountService.CreateAccountAsync(request);
            return Ok(new { Message = "Account created successfully" });
        }


        [HttpGet("my-accounts/{userId}")]
        public async Task<IActionResult> GetUserAccounts(int userId)
        {
            var accounts = await _accountService.GetUserByIdAsync(userId);
            return Ok(accounts);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDto request)
        {
            var newBalance = await _accountService.DepositAsync(request);
            return Ok(new { Message = "Deposit successful", NewBalance = newBalance });
        }
    }
}