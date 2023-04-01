using AutoMapper;
using BankSystem.Application.Dto;
using BankSystem.Application.Dto.Mapper;
using BankSystem.Core.Aggregate.Entities;

namespace BankSystem.Infrastructure.Automappers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegisterNewAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();
            CreateMap<TransactionRequestDto, Transaction>();

        }
    }
}
