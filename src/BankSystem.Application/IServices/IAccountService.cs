using BankSystem.Application.Dto;
using BankSystem.Core.Aggregate.Entities;

namespace BankSystem.Application.IServices
{
    public interface IAccountService
    {
        Task<Account> AuthenticateAsync(AuthenticateDto authenticateDto);
        Task<Account> AuthenticateAsync(string accountNumber, string pin);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> CreateAsync(CreateAccountDto createAccountDto);
        Task UpdateAsync(UpdateAccountPinDto updateAccountDto);
        Task DeleteAsync(int id);
        Task<Account> GetByIdAsync(int id);
        Task<Account> GetByAccountNumberAsync(string accountNumber);
    }
}
