using BankAPI.Contexts;
using BankAPI.Interfaces;
using BankAPI.Models;

namespace BankAPI.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankContext context) : base(context) { }
    }
}