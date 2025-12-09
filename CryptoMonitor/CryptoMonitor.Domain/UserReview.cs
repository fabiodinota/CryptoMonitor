using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMonitor.Domain
{
    public class UserReview
    {
        public int Id { get; set; }
        
        [Required]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        public string Comment { get; set; } = string.Empty;
        
        [Required]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public int Rating { get; set; }
        
        [Required]
        public DateTime DatePosted { get; set; }
        
        [Required]
        public int ExchangeId { get; set; }
        
        [Required]
        public Exchange Exchange { get; set; }

        public UserReview() { }

        public UserReview(int id, string userName, string comment, Exchange exchange)
        {
            Id = id;
            UserName = userName;
            Comment = comment;
            Exchange = exchange;
            DatePosted = DateTime.UtcNow;
            ExchangeId = exchange.Id;
        }

        public override string ToString()
        {
            return $"Review by {UserName} on {DatePosted:yyyy-MM-dd}: {Rating}/5 - \"{Comment}\"";
        }
    }
}
