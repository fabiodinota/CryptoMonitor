using CryptoMonitor.Domain;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitor.DAL.EF;

public static class DataSeeder
{
    public static void Seed(CryptoMonitorDbContext context)
    {
        if (context.Cryptocurrencies.Any())
        {
            return; 
        }
        
        // Exchanges
        var binance = new Exchange { Id = 1, Name = "Binance", TrustScore = 9, Website = "binance.com" };
        var coinbase = new Exchange { Id = 2, Name = "Coinbase", TrustScore = 10, Website = "coinbase.com" };
        var kraken = new Exchange { Id = 3, Name = "Kraken", TrustScore = 8, Website = "kraken.com" };
        var shadyDex = new Exchange { Id = 4, Name = "ShadyDex", TrustScore = 2, Website = "shady.io" };

        // Cryptos
        var btc = new Cryptocurrency { Id = 1, Name = "Bitcoin", Symbol = "BTC", CurrentPrice = 28500.50m, Type = CryptoType.Coin, MaxSupply = 21000000 };
        var eth = new Cryptocurrency { Id = 2, Name = "Ethereum", Symbol = "ETH", CurrentPrice = 1800.00m, Type = CryptoType.Coin, MaxSupply = 2 };
        var usdt = new Cryptocurrency { Id = 3, Name = "Tether", Symbol = "USDT", CurrentPrice = 1.00m, Type = CryptoType.Stablecoin, MaxSupply = 1 };
        var pepe = new Cryptocurrency { Id = 4, Name = "Pepe", Symbol = "PEPE", CurrentPrice = 0.000001m, Type = CryptoType.MemeCoin, MaxSupply = 420690000000000 };

        // User reviews
        var rev1 = new UserReview(1, "Alice", "Great UI", binance) { Rating = 4, DatePosted = System.DateTime.Now.AddDays(-5) };
        var rev2 = new UserReview(2, "Bob", "Fees too high", coinbase) { Rating = 1, DatePosted = System.DateTime.Now.AddDays(-1) };

        binance.Reviews.Add(rev1);
        coinbase.Reviews.Add(rev2);

        var listings = new List<ExchangeListing>
        {
            new ExchangeListing { Cryptocurrency = btc, Exchange = binance, ListingDate = DateTime.Now.AddYears(-5) },
            new ExchangeListing { Cryptocurrency = btc, Exchange = coinbase, ListingDate = DateTime.Now.AddYears(-4) },
            new ExchangeListing { Cryptocurrency = btc, Exchange = kraken, ListingDate = DateTime.Now.AddYears(-4) },
    
            new ExchangeListing { Cryptocurrency = pepe, Exchange = binance, ListingDate = DateTime.Now.AddMonths(-2) },
            new ExchangeListing { Cryptocurrency = pepe, Exchange = shadyDex, ListingDate = DateTime.Now.AddMonths(-6) },
    
            new ExchangeListing { Cryptocurrency = eth, Exchange = coinbase, ListingDate = DateTime.Now.AddYears(-3) }
        };

        context.AddRange(listings);
        
        context.Exchanges.AddRange(binance, coinbase, kraken, shadyDex);
        context.Cryptocurrencies.AddRange(btc, eth, usdt, pepe);
        
        context.SaveChanges();
        
        context.ChangeTracker.Clear();
    }
}
