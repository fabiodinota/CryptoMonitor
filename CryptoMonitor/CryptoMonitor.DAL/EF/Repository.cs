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

    public IEnumerable<Exchange> ReadAllExchangesWithCryptocurrenciesAndReviews()
    {
        return _context.Exchanges
            .Include(e => e.Listings)    
            .ThenInclude(l => l.Cryptocurrency)
            .Include(e => e.Reviews) 
            .AsEnumerable();
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
    
    public void AddListing(ExchangeListing listing)
    {
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