using BankSystem.Application.Dto;
using BankSystem.Core.Aggregate.Entities;

namespace BankSystem.Application.IServices
{
    public interface IAccountService
    {
        Task<Account> Authenticate(AuthenticateDto authenticateDto);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account> Create(CreateAccountDto createAccountDto);
        Task Update(UpdateAccountDto updateAccountDto);
        Task Delete();
        Task<Account> GetById(int id);
        Task<Account> GetByAccountNumber(string accountNumber);
    }
}
