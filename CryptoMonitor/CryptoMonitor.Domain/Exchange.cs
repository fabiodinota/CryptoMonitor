using System.Collections.Generic;

namespace CryptoMonitor.Domain
{
    public class Exchange
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public int TrustScore { get; set; } // Range 1-10

        public List<Cryptocurrency> Cryptocurrencies { get; set; } = new List<Cryptocurrency>();
        public List<UserReview> Reviews { get; set; } = new List<UserReview>();

        public override string ToString()
        {
            return $"Exchange: {Name} ({Website}) - Trust: {TrustScore}/10 - Listed Coins: {Cryptocurrencies.Count}";
        }
    }
}