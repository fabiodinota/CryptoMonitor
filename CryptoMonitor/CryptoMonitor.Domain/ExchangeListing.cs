using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoMonitor.Domain
{
    public class ExchangeListing
    {
        public int CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }

        public int ExchangeId { get; set; }
        public Exchange Exchange { get; set; }

        public DateTime ListingDate { get; set; }
        
        public override string ToString()
        {
            return $"{Cryptocurrency?.Symbol} on {Exchange?.Name} since {ListingDate:d}";
        }
    }
}