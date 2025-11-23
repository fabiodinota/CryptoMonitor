using CryptoMonitor.Domain;

namespace CryptoMonitor.DAL
{
    public class InMemoryRepository : IRepository
    {
        private static List<Cryptocurrency> _cryptos = new List<Cryptocurrency>();
        private static List<Exchange> _exchanges = new List<Exchange>();
        private static List<UserReview> _reviews = new List<UserReview>();

        public static void Seed()
        {
            // Prevent re-seeding if data already exists
            if (_cryptos.Any() || _exchanges.Any())
            {
                return;
            }

            // Exchanges
            var binance = new Exchange { Id = 1, Name = "Binance", TrustScore = 9, Website = "binance.com" };
            var coinbase = new Exchange { Id = 2, Name = "Coinbase", TrustScore = 10, Website = "coinbase.com" };
            var kraken = new Exchange { Id = 3, Name = "Kraken", TrustScore = 8, Website = "kraken.com" };
            var shadyDex = new Exchange { Id = 4, Name = "ShadyDex", TrustScore = 2, Website = "shady.io" };
            _exchanges.AddRange(new[] { binance, coinbase, kraken, shadyDex });

            // Cryptos
            var btc = new Cryptocurrency { Id = 1, Name = "Bitcoin", Symbol = "BTC", CurrentPrice = 28500.50, Type = CryptoType.Coin, MaxSupply = 21000000 };
            var eth = new Cryptocurrency { Id = 2, Name = "Ethereum", Symbol = "ETH", CurrentPrice = 1800.00, Type = CryptoType.Coin, MaxSupply = null };
            var usdt = new Cryptocurrency { Id = 3, Name = "Tether", Symbol = "USDT", CurrentPrice = 1.00, Type = CryptoType.Stablecoin, MaxSupply = null };
            var pepe = new Cryptocurrency { Id = 4, Name = "Pepe", Symbol = "PEPE", CurrentPrice = 0.000001, Type = CryptoType.MemeCoin, MaxSupply = 420690000000000 };
            _cryptos.AddRange(new[] { btc, eth, usdt, pepe });
            
            // User reviews
            var rev1 = new UserReview(1, "Alice", "Great UI", binance) { Rating = 4, DatePosted = System.DateTime.Now.AddDays(-5) };
            var rev2 = new UserReview(2, "Bob", "Fees too high", coinbase) { Rating = 1, DatePosted = System.DateTime.Now.AddDays(-1) };

            // Link reviews to exchanges
            binance.Reviews.Add(rev1);
            coinbase.Reviews.Add(rev2);

            // Link cryptos and exchanges
            btc.Exchanges.Add(binance); binance.Cryptocurrencies.Add(btc);
            btc.Exchanges.Add(coinbase); coinbase.Cryptocurrencies.Add(btc);
            btc.Exchanges.Add(kraken); kraken.Cryptocurrencies.Add(btc);

            pepe.Exchanges.Add(binance); binance.Cryptocurrencies.Add(pepe);
            pepe.Exchanges.Add(shadyDex); shadyDex.Cryptocurrencies.Add(pepe);

            eth.Exchanges.Add(coinbase); coinbase.Cryptocurrencies.Add(eth);
        }

        public void CreateCryptocurrency(Cryptocurrency cryptocurrency)
        {
            cryptocurrency.Id = _cryptos.Count + 1;
            _cryptos.Add(cryptocurrency);
        }

        public Cryptocurrency ReadCryptocurrency(int id)
        {
            return _cryptos.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Cryptocurrency> ReadAllCryptocurrencies()
        {
            return _cryptos;
        }

        public IEnumerable<Cryptocurrency> ReadCryptocurrenciesFiltered(CryptoType? type, Exchange exchange)
        {
            var query = _cryptos.AsQueryable();
            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }
            if (exchange != null)
            {
                query = query.Where(c => c.Exchanges.Contains(exchange));
            }
            return query.ToList();
        }

        public void CreateExchange(Exchange exchange)
        {
            exchange.Id = _exchanges.Count + 1;
            _exchanges.Add(exchange);
        }

        public Exchange ReadExchange(int id)
        {
            return _exchanges.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Exchange> ReadAllExchanges()
        {
            return _exchanges;
        }

        public IEnumerable<Exchange> ReadExchangesFiltered(string namePart, int? minTrustScore)
        {
            var query = _exchanges.AsQueryable();
            if (!string.IsNullOrEmpty(namePart))
            {
                query = query.Where(e => e.Name.ToLower().Contains(namePart.ToLower()));
            }
            if (minTrustScore.HasValue)
            {
                query = query.Where(e => e.TrustScore >= minTrustScore.Value);
            }
            return query.ToList();
        }
        
        public void CreateUserReview(UserReview review)
        {
            // Simulate auto-increment ID
            review.Id = _reviews.Any() ? _reviews.Max(r => r.Id) + 1 : 1;
            _reviews.Add(review);
        }
        
    }
}
