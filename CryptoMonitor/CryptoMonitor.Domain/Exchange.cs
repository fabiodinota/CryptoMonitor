using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMonitor.Domain
{
    public class Exchange
    {
        public int Id { get; set; }
        
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Website { get; set; }
        
        [Range(1, 10, ErrorMessage = "Trust Score must be between 1 and 10")]
        public int TrustScore { get; set; } 

        public List<ExchangeListing> Listings { get; set; } = new List<ExchangeListing>();        

        public List<UserReview> Reviews { get; set; } = new List<UserReview>();

        public override string ToString()
        {
            return $"Exchange: {Name} ({Website}) - Trust: {TrustScore}/10";
        }
    }
}
