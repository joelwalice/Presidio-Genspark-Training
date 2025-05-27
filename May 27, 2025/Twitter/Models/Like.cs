namespace Twitter.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
        public Tweet Tweet { get; set; } = null!;
    }
}