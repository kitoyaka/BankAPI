
using System.ComponentModel.DataAnnotations;

namespace Bank.Core.DTOs
{
    public class DepositDto
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }

    }
}