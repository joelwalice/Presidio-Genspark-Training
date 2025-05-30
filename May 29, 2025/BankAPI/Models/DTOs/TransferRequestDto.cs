namespace BankAPI.Models.DTOs
{
    public class TransferRequestDto
    {
        public string SourceAccountId { get; set; }
        public string TargetAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}