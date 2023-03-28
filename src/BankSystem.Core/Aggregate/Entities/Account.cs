using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Core.Aggregate.Entities
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public Guid AccountNumberGenerated { get; set; }
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        public Account()
        {
            AccountName = $"{FirstName}_{LastName}";
        }
    }

    public enum AccountType
    {
        Saving,
        Current,
        Corporate,
        Goverment
    }
}
