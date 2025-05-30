using System.Collections.Generic;
using System.Threading.Tasks;
using BankAPI.Interfaces;
using BankAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ActionResult<AccountResponseDto>> CreateAccount([FromBody] AccountCreateDto accountDto)
        {
            var createdAccount = await _accountService.CreateAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccountById), new { accountNumber = createdAccount.Id }, createdAccount);
        }

        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountResponseDto>> GetAccountById(string accountNumber)
        {
            var account = await _accountService.GetAccountAsync(accountNumber);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{accountNumber}/balance")]
        public async Task<ActionResult<decimal>> GetAccountBalance(string accountNumber)
        {
            var balance = await _accountService.GetAccountBalanceAsync(accountNumber);
            if (balance < 0)
                return NotFound();
            return Ok($"Your Balance is {balance}");
        }

    }
}