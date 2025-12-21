using Bank.Core.DTOs;
using Bank.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto request)
        {
            try
            {
                await _transactionService.TransferAsync(request);
                return Ok(new { Message = "Transfer successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("transactions/{accountId}")]
        public async Task<IActionResult> GetTransactionsForAccount(int accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsForAccountAsync(accountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}