using BankSystem.Core.Aggregate.Entities;

namespace BankSystem.Application.Dto
{
    public class UpdateAccountPinDto
    {
        public Account Account { get; set; }
        public string Pin { get; set; } = null;
    }
}
