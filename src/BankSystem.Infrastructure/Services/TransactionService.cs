using BankSystem.Application.Dto;
using BankSystem.Application.IServices;
using BankSystem.Core.Aggregate.Entities;
using BankSystem.Infrastructure.Persistence;
using BankSystem.Infrastructure.Units;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

namespace BankSystem.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private BankingDbContext context;
        ILogger<TransactionService> logger;
        private AppSettings settings;
        private static string ourBankSettlementAccount;
        private readonly IAccountService accountService;

        public TransactionService(
            BankingDbContext context,
            ILogger<TransactionService> logger,
            IOptions<AppSettings> settings,
            IAccountService accountService)
        {
            this.context = context;
            this.logger = logger;
            this.settings = settings.Value;
            ourBankSettlementAccount = this.settings.OurBankSettlementAccount;
            this.accountService = accountService;
        }

        public async Task<Response> CreateTransaction(Transaction transaction)
        {
            Response response = new Response();
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            response.Data = null;

            return response;
        }

        public async Task<Response> FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = context.Transactions.Where(x => x.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully";
            response.Data = transaction;

            return response;
        }

        public async Task<Response> MakeDeposit(DepositWithdrawalDto depositDto)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = await accountService.AuthenticateAsync(depositDto.authenticate);
            if (authUser == null) throw new ApplicationException("Invalid authentication");

            try 
            {
                sourceAccount = await accountService.GetByAccountNumberAsync(ourBankSettlementAccount);
                destinationAccount = await accountService.GetByAccountNumberAsync(depositDto.authenticate.AccountNumber);

                sourceAccount.CurrentAccountBalance -= depositDto.Amount;
                destinationAccount.CurrentAccountBalance += depositDto.Amount;

                if ((context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                   (context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = transaction;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = transaction;
                }

            }
            catch (Exception ex) 
            {
                logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = TransactionType.Deposit;
            transaction.TransactionSourceAccount = ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = depositDto.authenticate.AccountNumber;
            transaction.TransactionAmount = depositDto.Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $" ON DATE => {transaction.TransactionDate}" +
                $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"TRANSACTION TYPE => {transaction.TransactionType} " +
                $"TRANSACTION STATUS => {transaction.TransactionStatus}";

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            return response;              
        }

        public async Task<Response> MakeFundsTransfer(FundsTransferDto transferDto)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = await accountService.AuthenticateAsync(transferDto.FromAccount, transferDto.TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid authentication");

            try
            {
                sourceAccount = await accountService.GetByAccountNumberAsync(transferDto.FromAccount);
                destinationAccount = await accountService.GetByAccountNumberAsync(transferDto.ToAccount);

                sourceAccount.CurrentAccountBalance -= transferDto.Amount;
                destinationAccount.CurrentAccountBalance += transferDto.Amount;

                if ((context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                   (context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = transaction;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = transaction;
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = TransactionType.Transfer;
            transaction.TransactionSourceAccount = transferDto.FromAccount;
            transaction.TransactionDestinationAccount = transferDto.ToAccount;
            transaction.TransactionAmount = transferDto.Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $" ON DATE => {transaction.TransactionDate}" +
                $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"TRANSACTION TYPE => {transaction.TransactionType} " +
                $"TRANSACTION STATUS => {transaction.TransactionStatus}";

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            return response;
        }

        public async Task<Response> MakeWithdrawal(DepositWithdrawalDto withdrawalDto)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = await accountService.AuthenticateAsync(withdrawalDto.authenticate);
            if (authUser == null) throw new ApplicationException("Invalid authentication");

            try
            {
                sourceAccount = await accountService.GetByAccountNumberAsync(withdrawalDto.authenticate.AccountNumber);
                destinationAccount = await accountService.GetByAccountNumberAsync(ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= withdrawalDto.Amount;
                destinationAccount.CurrentAccountBalance += withdrawalDto.Amount;

                if ((context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                   (context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = transaction;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = transaction;
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = TransactionType.Withdrawal;
            transaction.TransactionSourceAccount = withdrawalDto.authenticate.AccountNumber;
            transaction.TransactionDestinationAccount = ourBankSettlementAccount;
            transaction.TransactionAmount = withdrawalDto.Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $" ON DATE => {transaction.TransactionDate}" +
                $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"TRANSACTION TYPE => {transaction.TransactionType} " +
                $"TRANSACTION STATUS => {transaction.TransactionStatus}";

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            return response;
        }
    }
}
