namespace BankAPI.Models.DTOs
{
    public class TransferRequestDto
    {
        public string SourceAccountNumber { get; set; }
        public string TargetAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}