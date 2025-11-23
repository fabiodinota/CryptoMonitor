using CryptoMonitor.Domain;

namespace CryptoMonitor.DAL;

public interface IRepository
{
    // Crypto abstract methods
    Cryptocurrency ReadCryptocurrency(int id);
    IEnumerable<Cryptocurrency> ReadAllCryptocurrencies();
    IEnumerable<Cryptocurrency> ReadCryptocurrenciesFiltered(CryptoType? type, Exchange? exchange);
    
    // Exchange abstract methods
    Exchange ReadExchange(int id);
    IEnumerable<Exchange> ReadAllExchanges();
    IEnumerable<Exchange> ReadExchangesFiltered(string namePart, int? minTrustScore);
    
    IEnumerable<Cryptocurrency> ReadAllCryptocurrenciesWithExchanges();
    Cryptocurrency ReadCryptocurrencyWithExchanges(int id);
    IEnumerable<Exchange> ReadAllExchangesWithCryptocurrenciesAndReviews();
    Exchange ReadExchangeWithCryptocurrenciesAndReviews(int id);
    
    void CreateCryptocurrency(Cryptocurrency cryptocurrency);
    void CreateExchange(Exchange exchange);
    void CreateUserReview(UserReview review);
    void AddListing(ExchangeListing listing);
    void RemoveListing(int exchangeId, int cryptoId);
}