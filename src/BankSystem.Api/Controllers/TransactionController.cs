using AutoMapper;
using BankSystem.Application.Dto;
using BankSystem.Application.IServices;
using BankSystem.Core.Aggregate.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class TransactionController : ControllerBase
    {
        private ITransactionService service;
        private IMapper mapper;

        public TransactionController(ITransactionService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("create_new_transaction")]
        public async Task<IActionResult> CreateNewTransaction([FromBody] TransactionRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            var transaction = mapper.Map<Transaction>(dto);
            var result = await service.CreateTransaction(transaction);
            return Ok(result);

        }

        [HttpPost]
        [Route("make_deposit")]
        public async Task<IActionResult> MakeDeposit([FromBody] DepositWithdrawalDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            var result = await service.MakeDeposit(dto);
            return Ok(result);

        }

        [HttpPost]
        [Route("make_withdrawal")]
        public async Task<IActionResult> MakeWithdrawal([FromBody] DepositWithdrawalDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            var result = await service.MakeWithdrawal(dto);
            return Ok(result);

        }

        [HttpPost]
        [Route("make_funds_transfer")]
        public async Task<IActionResult> MakeFundsTransfer([FromBody] FundsTransferDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            var result = await service.MakeFundsTransfer(dto);
            return Ok(result);
        }
    }
}
