using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Core.Aggregate.Entities
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public Guid TransactionUniqueReference { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }

        public TransactionStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TransactionStatus.Success);

        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public enum TransactionStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
