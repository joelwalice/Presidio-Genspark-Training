namespace Twitter.Models
{
    public class TweetHastag
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public int HastagId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Navigation properties
        public Tweet Tweet { get; set; } = null!;
        public Hastag Hastag { get; set; } = null!;
    }
}