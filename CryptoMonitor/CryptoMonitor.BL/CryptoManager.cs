using System.ComponentModel.DataAnnotations;
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
            var cryptocurrency = new Cryptocurrency
            {
                Name = name,
                Symbol = symbol,
                CurrentPrice = currentPrice,
                Type = type,
                MaxSupply = maxSupply,
                Exchanges = exchanges
            };
            
            var isValid = ValidateByTryValidateObject(cryptocurrency);

            if (isValid)
            {
                exchanges.ForEach(e => e.Cryptocurrencies.Add(cryptocurrency));

                _repository.CreateCryptocurrency(cryptocurrency);
                
                Console.WriteLine("Cryptocurrency added successfully!");
            }
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

            var exchange = new Exchange
            {
                Name = name,
                Website = website,
                TrustScore = trustScore,
                Cryptocurrencies = cryptocurrencies
            };

            var isValid = ValidateByTryValidateObject(exchange);

            if (isValid)
            {
                cryptocurrencies.ForEach(c => c.Exchanges.Add(exchange));

                _repository.CreateExchange(exchange);
                
                Console.WriteLine("Exchange added successfully!");
            }
        }

        public IEnumerable<Exchange> GetAllExchanges()
        {
            return _repository.ReadAllExchanges();
        }

        public IEnumerable<Exchange> GetExchangesFiltered(string namePart, int? minTrustScore)
        {
            return _repository.ReadExchangesFiltered(namePart, minTrustScore);
        }

        public void AddUserReview(string userName, string comment, int rating, Exchange exchange)
        {
            var review = new UserReview
            {
                UserName = userName,
                Comment = comment,
                Rating = rating,
                DatePosted = DateTime.Now,
                Exchange = exchange
            };
            
            var isValid = ValidateByTryValidateObject(review);
            

            if (isValid)
            {
                exchange.Reviews.Add(review);

                _repository.CreateUserReview(review);
                
                Console.WriteLine("User Review added successfully!");
            }
        }

        private static void ValidateByValidateObject(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            try
            {
                Validator.ValidateObject(obj, validationContext, validateAllProperties: true);
                Console.WriteLine("This object is valid (using ValidateObject)");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static bool ValidateByTryValidateObject(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext
                , validationResults, true);
            if (!isValid)
                foreach (var validationError in validationResults)
                    Console.WriteLine(validationError.ErrorMessage);
            return isValid;
        }
    }
}