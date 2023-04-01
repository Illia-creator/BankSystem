using System.ComponentModel.DataAnnotations;

namespace BankSystem.Application.Dto
{
    public class FundsTransferDto
    {
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$", ErrorMessage = "Account number must be 10-digit")] 
        public string FromAccount { get; set; }
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$", ErrorMessage = "Account number must be 10-digit")]
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }
}
