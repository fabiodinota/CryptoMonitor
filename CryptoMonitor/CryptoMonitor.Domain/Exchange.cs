using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CryptoMonitor.Domain
{
    public class Exchange
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Website { get; set; }
        
        [Range(1, 10, ErrorMessage = "Trust Score must be between 1 and 10")]
        public int TrustScore { get; set; } 

        public List<Cryptocurrency> Cryptocurrencies { get; set; } = new List<Cryptocurrency>();
        public List<UserReview> Reviews { get; set; } = new List<UserReview>();

        public override string ToString()
        {
            return $"Exchange: {Name} ({Website}) - Trust: {TrustScore}/10 - Listed Coins: {Cryptocurrencies.Count}";
        }
    }
}