
namespace Bank.Core.DTOs
{
    public class TransactionDto
    {
        public int transactionId {get; set; }
        public decimal Amount {get; set; }
        public DateTime Date {get; set; }
        public int SenderAccountId {get; set; }
        public int ReceiverAccountId {get; set; }
    }
}