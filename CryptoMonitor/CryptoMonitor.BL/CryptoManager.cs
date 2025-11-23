using CryptoMonitor.Domain;

namespace CryptoMonitor.BL
{
    public class CryptoManager
    {
        private List<Cryptocurrency> _cryptos;
        private List<Exchange> _exchanges;

        public CryptoManager()
        {
            _cryptos = new List<Cryptocurrency>();
            _exchanges = new List<Exchange>();
        }

        public void Seed()
        {
            // Cryptos
            var btc = new Cryptocurrency { Id = 1, Name = "Bitcoin", Symbol = "BTC", CurrentPrice = 28500.50, Type = CryptoType.Coin, MaxSupply = 21000000 };
            var eth = new Cryptocurrency { Id = 2, Name = "Ethereum", Symbol = "ETH", CurrentPrice = 1800.00, Type = CryptoType.Coin, MaxSupply = null };
            var usdt = new Cryptocurrency { Id = 3, Name = "Tether", Symbol = "USDT", CurrentPrice = 1.00, Type = CryptoType.Stablecoin, MaxSupply = null };
            var pepe = new Cryptocurrency { Id = 4, Name = "Pepe", Symbol = "PEPE", CurrentPrice = 0.000001, Type = CryptoType.MemeCoin, MaxSupply = 420690000000000 };

            // Exchanges
            var binance = new Exchange { Id = 1, Name = "Binance", TrustScore = 9, Website = "binance.com" };
            var coinbase = new Exchange { Id = 2, Name = "Coinbase", TrustScore = 10, Website = "coinbase.com" };
            var kraken = new Exchange { Id = 3, Name = "Kraken", TrustScore = 8, Website = "kraken.com" };
            var shadyDex = new Exchange { Id = 4, Name = "ShadyDex", TrustScore = 2, Website = "shady.io" };

            // User reviews
            var rev1 = new UserReview { Id = 1, UserName = "Alice", Rating = 4.5, Comment = "Great UI", DatePosted = DateTime.Now.AddDays(-5), Exchange = binance };
            var rev2 = new UserReview { Id = 2, UserName = "Bob", Rating = 1.0, Comment = "Fees too high", DatePosted = DateTime.Now.AddDays(-1), Exchange = coinbase };
            
            // Link reviews to exchanges
            binance.Reviews.Add(rev1);
            coinbase.Reviews.Add(rev2);

            // Add exchange listings to crypto
            btc.Exchanges.Add(binance); binance.Cryptocurrencies.Add(btc);
            btc.Exchanges.Add(coinbase); coinbase.Cryptocurrencies.Add(btc);
            btc.Exchanges.Add(kraken); kraken.Cryptocurrencies.Add(btc);

            pepe.Exchanges.Add(binance); binance.Cryptocurrencies.Add(pepe);
            pepe.Exchanges.Add(shadyDex); shadyDex.Cryptocurrencies.Add(pepe);

            eth.Exchanges.Add(coinbase); coinbase.Cryptocurrencies.Add(eth);

            _cryptos.AddRange(new[] { btc, eth, usdt, pepe });
            _exchanges.AddRange(new[] { binance, coinbase, kraken, shadyDex });
        }
        
        public IEnumerable<Cryptocurrency> GetAllCryptos()
        {
            return _cryptos;
        }

        public IEnumerable<Exchange> GetAllExchanges()
        {
            return _exchanges;
        }

        // filtering
        public IEnumerable<Cryptocurrency> GetCryptosByType(CryptoType type)
        {
            return _cryptos.Where(c => c.Type == type);
        }

        public IEnumerable<Exchange> GetExchangesFiltered(string namePart, int? minTrustScore)
        {
            var query = _exchanges.AsEnumerable();

            // optional filter 
            if (!string.IsNullOrEmpty(namePart))
            {
                query = query.Where(e => e.Name.ToLower().Contains(namePart.ToLower()));
            }

            // optional filter
            if (minTrustScore.HasValue)
            {
                query = query.Where(e => e.TrustScore >= minTrustScore.Value);
            }

            return query;
        }
    }
}