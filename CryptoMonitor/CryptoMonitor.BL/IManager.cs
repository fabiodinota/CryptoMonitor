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
    Cryptocurrency GetCryptocurrencyWithExchanges(int id);
    IEnumerable<Exchange> GetAllExchangesWithDetails();
    Exchange GetExchangeWithDetails(int id);
    
    IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> AddCryptocurrency(
        string name,
        string symbol,
        decimal currentPrice,
        CryptoType type,
        long? maxSupply,
        List<Exchange> exchanges
    );
    IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> AddExchange(
        string name,
        string website,
        int trustScore,
        List<Cryptocurrency> cryptocurrencies
    );
    IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> AddUserReview(
        string userName,
        string comment,
        int rating,
        Exchange exchange
    );

    IEnumerable<UserReview> GetAllUserReviews();
    
    void AddListing(int exchangeId, int cryptoId, DateTime listingDate);
    void RemoveListing(int exchangeId, int cryptoId);
}
