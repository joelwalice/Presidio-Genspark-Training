namespace BankAPI.Models.DTOs
{
    public class WithdrawRequestDto
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}