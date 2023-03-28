namespace BankSystem.Application.Dto
{
    public class FundsTransferDto
    { 
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPin { get; set; }
    }
}
