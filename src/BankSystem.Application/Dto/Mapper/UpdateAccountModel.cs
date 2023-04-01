using System.ComponentModel.DataAnnotations;

namespace BankSystem.Application.Dto.Mapper
{
    public class UpdateAccountModel
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }

        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }

    }
}
