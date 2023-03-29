using BankSystem.Core.Aggregate.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankSystem.Application.Dto.Mapper
{
    public class RegisterNewAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]\d[4]$", ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }

        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }
    }
}
