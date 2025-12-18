
using System.ComponentModel.DataAnnotations;

namespace Bank.Core.DTOs
{
    public class TransferDto
    {
        [Required]
        public int SenderAccountId { get; set; }
        [Required]
        public int ReceiverAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}