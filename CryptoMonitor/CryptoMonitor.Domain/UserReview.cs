namespace CryptoMonitor.Domain
{
    public class UserReview
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }
        public DateTime DatePosted { get; set; }
        
        public Exchange Exchange { get; set; }

        public UserReview() { }

        public UserReview(int id, string userName, string comment, Exchange exchange)
        {
            Id = id;
            UserName = userName;
            Comment = comment;
            Exchange = exchange;
        }

        public override string ToString()
        {
            return $"Review by {UserName} on {DatePosted:yyyy-MM-dd}: {Rating}/5 - \"{Comment}\"";
        }
    }
}