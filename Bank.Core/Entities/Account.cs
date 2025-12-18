namespace Bank.Core.Entities
{
public class Account
{
    public int Id { get; set; }
    public string IBAN {get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string currency { get; set; } = "UAH";
    public int UserId { get; set; }
    public User? User { get; set; }

}
}