using CryptoMonitor.Domain;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitor.DAL.EF;

public class Repository : IRepository
{
    private readonly CryptoMonitorDbContext _context;
    
    public Repository(CryptoMonitorDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Cryptocurrency> ReadAllCryptocurrencies()
    {
        // Eager Loading: Laad Listings én de Exchange die erachter zit
        return _context.Cryptocurrencies
            .Include(c => c.Listings)
            .ThenInclude(l => l.Exchange)
            .AsEnumerable();
    }

    public IEnumerable<Exchange> ReadAllExchanges()
    {
        // Eager Loading: Laad Listings + Crypto EN Reviews
        return _context.Exchanges
            .Include(e => e.Listings)
            .ThenInclude(l => l.Cryptocurrency)
            .Include(e => e.Reviews) // Laad ook reviews mee
            .AsEnumerable();
    }

    public IEnumerable<Cryptocurrency> ReadAllCryptocurrenciesWithExchanges()
    {
        return _context.Cryptocurrencies
            .Include(c => c.Listings)  
            .ThenInclude(l => l.Exchange) 
            .AsEnumerable();
    }

    public Cryptocurrency ReadCryptocurrencyWithExchanges(int id)
    {
        return _context.Cryptocurrencies
            .Include(c => c.Listings)
            .ThenInclude(l => l.Exchange)
            .FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Exchange> ReadAllExchangesWithCryptocurrenciesAndReviews()
    {
        return _context.Exchanges
            .Include(e => e.Listings)    
            .ThenInclude(l => l.Cryptocurrency)
            .Include(e => e.Reviews) 
            .AsEnumerable();
    }
    
    public Exchange ReadExchangeWithCryptocurrenciesAndReviews(int id)
    {
        return _context.Exchanges
            .Include(e => e.Listings)
            .ThenInclude(l => l.Cryptocurrency) // Laad de crypto info
            .Include(e => e.Reviews)                // Laad de reviews
            .FirstOrDefault(e => e.Id == id);
    }

    public void CreateCryptocurrency(Cryptocurrency cryptocurrency)
    {
        _context.Cryptocurrencies.Add(cryptocurrency);
        _context.SaveChanges();
    }

    public Cryptocurrency ReadCryptocurrency(int id)
    {
        return _context.Cryptocurrencies.FirstOrDefault(c => c.Id == id);
    }

   
// Update ook je filter methodes!
    public IEnumerable<Cryptocurrency> ReadCryptocurrenciesFiltered(CryptoType? type, Exchange exchange)
    {
        var query = _context.Cryptocurrencies
            .Include(c => c.Listings)
            .ThenInclude(l => l.Exchange)
            .AsQueryable();

        if (type.HasValue)
        {
            query = query.Where(c => c.Type == type.Value);
        }
    
        if (exchange != null)
        {
            // Nu filteren we op de Listings tabel!
            query = query.Where(c => c.Listings.Any(l => l.ExchangeId == exchange.Id));
        }

        return query.ToList();
    }

    public Exchange ReadExchange(int id)
    {
        return _context.Exchanges.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Exchange> ReadExchangesFiltered(string namePart, int? minTrustScore)
    {
        var query = _context.Exchanges.AsQueryable();

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
    public void CreateExchange(Exchange exchange)
    {
        _context.Exchanges.Add(exchange);
        _context.SaveChanges();
    }

    public void CreateUserReview(UserReview review)
    {
        _context.UserReviews.Add(review);
        _context.SaveChanges();
    }

    public IEnumerable<UserReview> ReadAllUserReviews()
    {
        return _context.UserReviews
            .Include(r => r.Exchange)
            .AsEnumerable();
    }
    
    public void AddListing(ExchangeListing listing)
    {
        bool exchangeExists = _context.Exchanges.Any(e => e.Id == listing.ExchangeId);
        bool cryptoExists = _context.Cryptocurrencies.Any(c => c.Id == listing.CryptocurrencyId);

        if (!exchangeExists)
        {
            throw new ArgumentException("Exchange not found.", nameof(listing.ExchangeId));
        }

        if (!cryptoExists)
        {
            throw new ArgumentException("Cryptocurrency not found.", nameof(listing.CryptocurrencyId));
        }

        bool alreadyListed = _context.Set<ExchangeListing>()
            .Any(l => l.ExchangeId == listing.ExchangeId && l.CryptocurrencyId == listing.CryptocurrencyId);

        if (alreadyListed)
        {
            throw new InvalidOperationException("This cryptocurrency is already listed on the exchange.");
        }

        _context.Set<ExchangeListing>().Add(listing);
        _context.SaveChanges();
    }

    public void RemoveListing(int exchangeId, int cryptoId)
    {
        var listing = _context.Set<ExchangeListing>().Find(cryptoId, exchangeId);
        if (listing != null)
        {
            _context.Set<ExchangeListing>().Remove(listing);
            _context.SaveChanges();
        }
    }
}
