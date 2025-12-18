using System.ComponentModel.DataAnnotations;

namespace Bank.Core.DTOs
{
    public class CreateAccountDto
    {
        [Required]
        public int UserId{get;set;}

        [Required]
        public string Currency {get;set;} = "UAH";
    }
}