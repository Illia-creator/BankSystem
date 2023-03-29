using BankSystem.Application.Dto;
using BankSystem.Application.IServices;
using BankSystem.Core.Aggregate.Entities;
using BankSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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

        private static bool VerifyPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(pin)) throw new ArgumentNullException("Pin");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if(computedPinHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<Account> Authenticate(AuthenticateDto authenticateDto)
        {
            var account = context.Accounts.Where(x => x.AccountNumberGenerated == authenticateDto.AccountNumber).SingleOrDefault();
            if (account == null) return null;
            if (!VerifyPinHash(authenticateDto.Pin, account.PinHash, account.PinSalt)) return null;

            return account;
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


            return createAccountDto.Account;
        }

        public async Task Delete(int id)
        {
            var account = await context.Accounts.FindAsync(id);
            if (account == null) throw new ApplicationException($"Account with id: {id} does not exist");
            else 
            {
                context.Accounts.Remove(account);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await context.Accounts.ToListAsync();
        }

        public async Task<Account> GetByAccountNumber(Guid accountNumber)
        {
            var account = await context.Accounts.FirstOrDefaultAsync(x => x.AccountNumberGenerated == accountNumber);
            if (account == null) return null;
            else return account;
        }

        public async Task<Account> GetById(int id)
        {
            var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (account == null) return null;
            else return account;
        }

        public Task Update(UpdateAccountDto updateAccountDto)
        {
            throw new NotImplementedException();
        }
    }
}
