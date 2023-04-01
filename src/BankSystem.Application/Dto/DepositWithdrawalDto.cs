namespace BankSystem.Application.Dto
{
    public class DepositWithdrawalDto
    {
        public AuthenticateDto authenticate { get; set; }
        public decimal Amount { get; set; }
    }
}
