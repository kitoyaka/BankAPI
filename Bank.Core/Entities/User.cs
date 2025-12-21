using System.Collections.Generic;

namespace Bank.Core.Entities
{
public class User
{
    public int Id { get; set; }
    public required string  FullName { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Password { get; set; }

    public Account? Account { get; set; } 
    public bool IsDeleted { get; set; } = false;

}
}