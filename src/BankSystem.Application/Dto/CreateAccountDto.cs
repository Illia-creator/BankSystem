using BankSystem.Core.Aggregate.Entities;

namespace BankSystem.Application.Dto
{
    public class CreateAccountDto
    {
        public Account Account { get; set; }
        public string Pin { get; set; }
        public string ConfinmPin { get; set; }
    }
}
