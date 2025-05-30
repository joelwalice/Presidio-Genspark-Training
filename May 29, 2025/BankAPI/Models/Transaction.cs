using System;

namespace BankAPI.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } 
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TargetAccountNumber { get; set; }
    }
}