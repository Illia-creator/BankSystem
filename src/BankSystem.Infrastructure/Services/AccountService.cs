using BankSystem.Application.Dto;
using BankSystem.Application.IServices;
using BankSystem.Core.Aggregate.Entities;
using BankSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        public async Task<Account> AuthenticateAsync(AuthenticateDto authenticateDto)
        {
            var account = context.Accounts.Where(x => x.AccountNumberGenerated == authenticateDto.AccountNumber).SingleOrDefault();
            if (account == null) return null;
            if (!VerifyPinHash(authenticateDto.Pin, account.PinHash, account.PinSalt)) return null;

            return account;
        }
        public async Task<Account> AuthenticateAsync(string accountNumber, string pin)
        {
            var account = context.Accounts.Where(x => x.AccountNumberGenerated == accountNumber).SingleOrDefault();
            if (account == null) return null;
            if (!VerifyPinHash(pin, account.PinHash, account.PinSalt)) return null;

            return account;
        }

        public async Task<Account> CreateAsync(CreateAccountDto createAccountDto)
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

        public async Task DeleteAsync(int id)
        {
            var account = await context.Accounts.FindAsync(id);
            if (account == null) throw new ApplicationException($"Account with id: {id} does not exist");
            else 
            {
                context.Accounts.Remove(account);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await context.Accounts.ToListAsync();
        }

        public async Task<Account> GetByAccountNumberAsync(string accountNumber)
        {
            var account = await context.Accounts.FirstOrDefaultAsync(x => x.AccountNumberGenerated == accountNumber);
            if (account == null) return null;
            else return account;
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (account == null) return null;
            else return account;
        }

        public async Task UpdateAsync(UpdateAccountPinDto updateAccountDto)
        {
            var accountToUpdate = await context.Accounts.FirstOrDefaultAsync(x => x.Email == updateAccountDto.Account.Email);
            if (accountToUpdate == null) throw new ApplicationException("Accont does not exist");

            if (!string.IsNullOrWhiteSpace(updateAccountDto.Account.Email))
            {
                if (await context.Accounts.AnyAsync(x => x.Email == updateAccountDto.Account.Email)) throw new ApplicationException($"Email \"{updateAccountDto.Account.Email}\" already exist");
                else accountToUpdate.Email = updateAccountDto.Account.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateAccountDto.Account.PhoneNumber))
            {
                if (await context.Accounts.AnyAsync(x => x.PhoneNumber == updateAccountDto.Account.PhoneNumber)) throw new ApplicationException($"Phone Number {updateAccountDto.Account.PhoneNumber} already exist");
                else accountToUpdate.PhoneNumber = updateAccountDto.Account.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(updateAccountDto.Pin))
            {
                byte[] pinHash, pinSalt;

                CreatePinHash(updateAccountDto.Pin, out pinHash, out pinSalt);

                accountToUpdate.PinHash = pinHash;
                accountToUpdate.PinSalt = pinSalt;
            }
            accountToUpdate.DateLastUpdated = DateTime.Now;

            context.Accounts.Update(accountToUpdate);
            await context.SaveChangesAsync();
        }
    }
}
