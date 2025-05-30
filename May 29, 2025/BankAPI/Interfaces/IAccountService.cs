using System.Collections.Generic;
using System.Threading.Tasks;
using BankAPI.Models.DTOs;

namespace BankAPI.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponseDto> CreateAccountAsync(AccountCreateDto dto);
        Task<AccountResponseDto> GetAccountAsync(string accountId);
        Task<IEnumerable<AccountResponseDto>> GetAllAccountsAsync();
        Task<decimal> GetAccountBalanceAsync(string accountId);
    }
}