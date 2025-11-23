using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMonitor.Domain
{
    public class UserReview
    {
        public int Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Comment { get; set; }
        
        [Required]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public int Rating { get; set; }
        
        [Required]
        public DateTime DatePosted { get; set; }
        
        [Required]
        [NotMapped]

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