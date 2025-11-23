using CryptoMonitor.Domain;

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
        return _context.Cryptocurrencies.AsEnumerable();
    }

    public IEnumerable<Exchange> ReadAllExchanges()
    {
        return _context.Exchanges.AsEnumerable();
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

    public IEnumerable<Cryptocurrency> ReadCryptocurrenciesFiltered(CryptoType? type, Exchange exchange)
    {
        var query = _context.Cryptocurrencies.AsQueryable();

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
}