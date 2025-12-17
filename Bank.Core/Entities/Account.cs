namespace Bank.Core.Entities
{
public class Account
{
    public int Id { get; set; }
    public int IBAN {get; set; }
    public decimal Balance { get; set; }
    public string currency { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

}
}