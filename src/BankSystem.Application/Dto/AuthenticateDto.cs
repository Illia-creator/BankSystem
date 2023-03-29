using System.ComponentModel.DataAnnotations;

namespace BankSystem.Application.Dto
{
    public class AuthenticateDto
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|[1-9]\d{9}$")]
        public Guid AccountNumber { get; set; }
        [Required]
        public string Pin { get; set; }
    }
}
