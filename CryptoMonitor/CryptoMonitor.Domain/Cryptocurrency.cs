using System.Collections.Generic;

namespace CryptoMonitor.Domain
{
    public class Cryptocurrency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double CurrentPrice { get; set; }
        public CryptoType Type { get; set; }
        public long? MaxSupply { get; set; }

        // nav props (*-*)
        public List<Exchange> Exchanges { get; set; } = new List<Exchange>();

        public override string ToString()
        {
            string supply = 
                MaxSupply.HasValue 
                    ? MaxSupply.Value.ToString("N0") 
                    : "Infinite";
            
            return $"{Name} ({Symbol}) - €{CurrentPrice:N2} [{Type}] | Supply: {supply}";
        }
    }
}