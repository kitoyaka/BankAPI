namespace Bank.Core.Entities
{
public class Account
{
    public int Id { get; set; }
    public string IBAN {get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "UAH";
    public int UserId { get; set; }
    public User? User { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void Debit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception("Amount to debit must be greater than zero.");
        }
        if (Balance < amount)
        {
            throw new Exception("Insufficient funds.");
        }
        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception("Amount to credit must be greater than zero.");
        }
        Balance += amount;
    }

}
}