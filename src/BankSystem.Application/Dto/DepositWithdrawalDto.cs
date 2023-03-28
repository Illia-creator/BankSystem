namespace BankSystem.Application.Dto
{
    public class DepositWithdrawalDto
    {
        public string AccounyNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }
}
