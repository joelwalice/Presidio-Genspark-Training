namespace Twitter.Models
{
    public class Hastag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
        public ICollection<TweetHastag> TweetHastags { get; set; } = new List<TweetHastag>();
    }
}