namespace BankAPI.Models.DTOs
{
    public class AccountCreateDto
    {
        // public string AccountNumber { get; set; } = string.Empty;
        public string HolderName { get; set; } = string.Empty;
        public decimal InitialDeposit { get; set; }
    }
}