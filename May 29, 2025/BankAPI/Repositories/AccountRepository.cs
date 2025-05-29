using BankAPI.Contexts;
using BankAPI.Interfaces;
using BankAPI.Models;

namespace BankAPI.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(BankContext context) : base(context) { }
    }
}