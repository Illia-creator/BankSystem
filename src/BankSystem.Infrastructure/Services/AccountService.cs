using BankSystem.Application.Dto;
using BankSystem.Application.IServices;
using BankSystem.Core.Aggregate.Entities;
using BankSystem.Infrastructure.Persistence;
using System.Text;

namespace BankSystem.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingDbContext context;
        public AccountService(BankingDbContext context)
        {
            this.context = context;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public Task<Account> Authenticate(AuthenticateDto authenticateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> Create(CreateAccountDto createAccountDto)
        {
            if (context.Accounts.Any(x => x.Email == createAccountDto.Account.Email)) throw new ApplicationException("Account already exist with this email!");
            if (!createAccountDto.Pin.Equals(createAccountDto.ConfinmPin)) throw new ArgumentException("Pins do not match");

            byte[] pinHash, pinSalt;

            CreatePinHash(createAccountDto.Pin, out pinHash, out pinSalt);

            createAccountDto.Account.PinHash = pinHash;
            createAccountDto.Account.PinSalt = pinSalt;

            context.Accounts.Add(createAccountDto.Account);
            await context.SaveChangesAsync();
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetByAccountNumber(string accountNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(UpdateAccountDto updateAccountDto)
        {
            throw new NotImplementedException();
        }
    }
}
