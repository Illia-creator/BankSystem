using BankSystem.Core.Aggregate.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankSystem.Application.Dto
{
    public class TransactionRequestDto
    {
        public decimal TransactionAmount { get; set; }
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
