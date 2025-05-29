using BankAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAPI.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetAsync(int id); 
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction> AddAsync(Transaction transaction);
    }
}