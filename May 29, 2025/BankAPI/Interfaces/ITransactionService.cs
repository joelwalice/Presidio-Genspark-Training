using System.Threading.Tasks;
using BankAPI.Models.DTOs;

namespace BankAPI.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> DepositAsync(DepositRequestDto dto); 
        Task<bool> WithdrawAsync(WithdrawRequestDto dto);
        Task<bool> TransferAsync(TransferRequestDto dto);
    }
}