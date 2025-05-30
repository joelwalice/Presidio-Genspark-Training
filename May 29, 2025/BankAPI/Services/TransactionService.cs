using System;
using System.Threading.Tasks;
using BankAPI.Interfaces;
using BankAPI.Models;
using BankAPI.Models.DTOs;

namespace BankAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<bool> DepositAsync(DepositRequestDto dto)
        {
            var account = await _accountRepository.GetAsync(dto.AccountNumber);
            if (account == null)
                return false;

            account.Balance += dto.Amount;
            await _accountRepository.UpdateAsync(account);

            var transaction = new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = dto.Amount,
                Type = TransactionType.Deposit,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            return true;
        }

        public async Task<bool> WithdrawAsync(WithdrawRequestDto dto)
        {
            var account = await _accountRepository.GetAsync(dto.AccountNumber);
            if (account == null || account.Balance < dto.Amount)
                return false;

            account.Balance -= dto.Amount;
            await _accountRepository.UpdateAsync(account);

            var transaction = new Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = dto.Amount,
                Type = TransactionType.Withdrawal,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            return true;
        }

        public async Task<bool> TransferAsync(TransferRequestDto dto)
        {
            var sourceAccount = await _accountRepository.GetAsync(dto.SourceAccountNumber);
            var targetAccount = await _accountRepository.GetAsync(dto.TargetAccountNumber);
            if (sourceAccount == null || targetAccount == null || sourceAccount.Balance < dto.Amount)
                return false;

            sourceAccount.Balance -= dto.Amount;
            targetAccount.Balance += dto.Amount;
            await _accountRepository.UpdateAsync(sourceAccount);
            await _accountRepository.UpdateAsync(targetAccount);

            var transaction = new Transaction
            {
                AccountNumber = sourceAccount.AccountNumber,
                Amount = dto.Amount,
                Type = TransactionType.Transfer,
                TransactionDate = DateTime.UtcNow,
                TargetAccountNumber = targetAccount.AccountNumber
            };

            await _transactionRepository.AddAsync(transaction);
            return true;
        }
    }
}