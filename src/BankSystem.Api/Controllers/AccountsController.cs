using AutoMapper;
using BankSystem.Application.Dto;
using BankSystem.Application.Dto.Mapper;
using BankSystem.Application.IServices;
using BankSystem.Core.Aggregate.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountsController : ControllerBase
    {
        private IAccountService service;
        private IMapper mapper;

        public AccountsController(IAccountService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("register_new_account")]
        public async Task<IActionResult> RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
        {
            if (!ModelState.IsValid) return BadRequest(newAccount);

            var account = mapper.Map<Account>(newAccount);
            var accountToCreate = new CreateAccountDto()
            {
                Account = account,
                Pin = newAccount.Pin,
                ConfinmPin = newAccount.ConfirmPin
            };

            var result = await service.CreateAsync(accountToCreate);
            return Ok(result);
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await service.GetAllAccountsAsync();
            var clearAccounts = mapper.Map<IList<GetAccountModel>>(accounts);
            return Ok(clearAccounts);
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            var result = await service.AuthenticateAsync(dto);
            return Ok(result);
        }

    }
}
