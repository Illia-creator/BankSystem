using BankSystem.Application.Dto;
using BankSystem.Core.Aggregate.Entities;

namespace BankSystem.Application.IServices
{
    public interface ITransactionService
    {
        Task<Response> CreateTransaction(Transaction transaction);
        Task<Response> FindTransactionByDate(DateTime date);
        Task<Response> MakeDeposit(DepositWithdrawalDto depositDto);
        Task<Response> MakeWithdrawal(DepositWithdrawalDto withdrawalDto);
        Task<Response> MakeFundsTransfer(FundsTransferDto transferDto);
    }
}
