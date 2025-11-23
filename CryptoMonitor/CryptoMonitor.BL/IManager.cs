using CryptoMonitor.Domain;

namespace CryptoMonitor.BL;

public interface IManager
{
    // Crypto abstract methods
    Cryptocurrency GetCryptocurrency(int id);
    IEnumerable<Cryptocurrency> GetAllCryptocurrencies();
    IEnumerable<Cryptocurrency> GetCryptocurrenciesFiltered(CryptoType type, Exchange exchange);
    
    // Exchange abstract methods
    Exchange GetExchange(int id);
    IEnumerable<Exchange> GetAllExchanges();
    IEnumerable<Exchange> GetExchangesFiltered(string namePart, int? minTrustScore);
    
    IEnumerable<Cryptocurrency> GetAllCryptocurrenciesWithExchanges();
    IEnumerable<Exchange> GetAllExchangesWithDetails();
    
    void AddCryptocurrency(
        string name, 
        string symbol, 
        double currentPrice, 
        CryptoType type, 
        long? maxSupply, 
        List<Exchange> exchanges
        );
    void AddExchange(
        string name, 
        string website, 
        int trustScore, 
        List<Cryptocurrency> cryptocurrencies
        );
    void AddUserReview(
        string userName, 
        string comment, 
        int rating, 
        Exchange exchange
        );
    
    void AddListing(int exchangeId, int cryptoId);
    void RemoveListing(int exchangeId, int cryptoId);
}