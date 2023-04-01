using AutoMapper;
using BankSystem.Application.Dto;
using BankSystem.Application.Dto.Mapper;
using BankSystem.Application.IServices;
using BankSystem.Core.Aggregate.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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

        [HttpGet]
        [Route("account_by_number")]
        public async Task<IActionResult> GetAccountByNumber(string number)
        {
            if (!Regex.IsMatch(number, @"^[0][0-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10-digit");

            var account = await service.GetByAccountNumberAsync(number);
            var clearAccount = mapper.Map<GetAccountModel>(account);
            return Ok(clearAccount);
        }

        [HttpGet]
        [Route("account_by_id")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await service.GetByIdAsync(id);
            var clearAccount = mapper.Map<GetAccountModel>(account);
            return Ok(clearAccount);
        }

        [HttpPut]
        [Route("update-account")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            UpdateAccountPinDto updateAccount = mapper.Map<UpdateAccountPinDto>(model);

            await service.UpdateAsync(updateAccount);

            return Ok();
        }
    }
}
