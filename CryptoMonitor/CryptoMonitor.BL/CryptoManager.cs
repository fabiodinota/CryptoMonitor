using CryptoMonitor.DAL;
using CryptoMonitor.Domain;

namespace CryptoMonitor.BL
{
    public class CryptoManager : IManager
    {
        
        private readonly IRepository _repository;

        public CryptoManager(IRepository repositoryProp)
        {
            _repository = repositoryProp;
        }

        public Cryptocurrency GetCryptocurrency(int id)
        {
            return _repository.ReadCryptocurrency(id);
        }

        public void AddCryptocurrency(string name, string symbol, double currentPrice, CryptoType type, long? maxSupply,
            List<Exchange> exchanges)
        {
            _repository.CreateCryptocurrency(
                new Cryptocurrency
                {
                    Name = name, 
                    Symbol = symbol, 
                    CurrentPrice = currentPrice, 
                    Type = type, 
                    MaxSupply = maxSupply, 
                    Exchanges = exchanges
                });
        }

        public IEnumerable<Cryptocurrency> GetAllCryptocurrencies()
        {
            return _repository.ReadAllCryptocurrencies();
        }

        public IEnumerable<Cryptocurrency> GetCryptocurrenciesFiltered(CryptoType type, Exchange exchange)
        {
            return _repository.ReadCryptocurrenciesFiltered(type, exchange);
        }

        public Exchange GetExchange(int id)
        {
            return _repository.ReadExchange(id);
        }

        public void AddExchange(string name, string website, int trustScore, List<Cryptocurrency> cryptocurrencies)
        {
            _repository.CreateExchange(
                new Exchange
                {
                    Name = name, 
                    Website = website, 
                    TrustScore = trustScore, 
                    Cryptocurrencies = cryptocurrencies
                });
        }

        public IEnumerable<Exchange> GetAllExchanges()
        {
            return _repository.ReadAllExchanges();
        }

        public IEnumerable<Exchange> GetExchangesFiltered(string namePart, int? minTrustScore)
        {
            return _repository.ReadExchangesFiltered(namePart, minTrustScore);
        }
        
        public void AddUserReview(string userName, string comment, double rating, Exchange exchange)
        {
            var review = new UserReview
            {
                UserName = userName,
                Comment = comment,
                Rating = rating,
                DatePosted = DateTime.Now,
                Exchange = exchange
            };
            
            exchange.Reviews.Add(review);
            
            _repository.CreateUserReview(review); 
        }
    }
}