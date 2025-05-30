using BankAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAPI.Interfaces
{
    public interface IAccountRepository : IRepository<string, Account>
    {
    }
}