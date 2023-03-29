using BankSystem.Application.Dto;
using BankSystem.Core.Aggregate.Entities;

namespace BankSystem.Application.IServices
{
    public interface IAccountService
    {
        Task<Account> AuthenticateAsync(AuthenticateDto authenticateDto);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> CreateAsync(CreateAccountDto createAccountDto);
        Task UpdateAsync(UpdateAccountDto updateAccountDto);
        Task DeleteAsync(int id);
        Task<Account> GetByIdAsync(int id);
        Task<Account> GetByAccountNumberAsync(Guid accountNumber);
    }
}
