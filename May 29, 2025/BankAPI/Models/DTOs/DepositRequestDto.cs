namespace BankAPI.Models.DTOs
{
    public class DepositRequestDto
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}