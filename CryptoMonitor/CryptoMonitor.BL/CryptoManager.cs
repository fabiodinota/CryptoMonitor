using System;
using System.Collections.Generic;
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

        public IEnumerable<Exchange> GetAllExchanges()
        {
            return _repository.ReadAllExchanges();
        }

        public IEnumerable<Exchange> GetExchangesFiltered(string namePart, int? minTrustScore)
        {
            return _repository.ReadExchangesFiltered(namePart, minTrustScore);
        }
        
        public IEnumerable<Cryptocurrency> GetAllCryptocurrenciesWithExchanges()
        {
            return _repository.ReadAllCryptocurrenciesWithExchanges();
        }

        public Cryptocurrency GetCryptocurrencyWithExchanges(int id)
        {
            return _repository.ReadCryptocurrencyWithExchanges(id);
        }

        public IEnumerable<Exchange> GetAllExchangesWithDetails()
        {
            return _repository.ReadAllExchangesWithCryptocurrenciesAndReviews();
        }
        
        public Exchange GetExchangeWithDetails(int id)
        {
            return _repository.ReadExchangeWithCryptocurrenciesAndReviews(id);
        }

        public IEnumerable<ValidationResult> AddCryptocurrency(
            string name,
            string symbol,
            decimal currentPrice,
            CryptoType type,
            long? maxSupply,
            List<Exchange> exchanges)
        {
            var cryptocurrency = new Cryptocurrency
            {
                Name = name,
                Symbol = symbol,
                CurrentPrice = currentPrice,
                Type = type,
                MaxSupply = maxSupply
            };

            var validationResults = ValidateByTryValidateObject(cryptocurrency);

            if (!validationResults.Any())
            {
                foreach (var exchange in exchanges)
                {
                    var listing = new ExchangeListing
                    {
                        Cryptocurrency = cryptocurrency,
                        ExchangeId = exchange.Id,
                        ListingDate = DateTime.Now
                    };
                    
                    cryptocurrency.Listings.Add(listing);
                }

                _repository.CreateCryptocurrency(cryptocurrency);
            }

            return validationResults;
        }

        public IEnumerable<ValidationResult> AddExchange(
            string name,
            string website,
            int trustScore,
            List<Cryptocurrency> cryptocurrencies)
        {
            var exchange = new Exchange
            {
                Name = name,
                Website = website,
                TrustScore = trustScore
            };

            var validationResults = ValidateByTryValidateObject(exchange);

            if (!validationResults.Any())
            {
                foreach (var crypto in cryptocurrencies)
                {
                    var listing = new ExchangeListing
                    {
                        Exchange = exchange,
                        CryptocurrencyId = crypto.Id,
                        ListingDate = DateTime.Now
                    };
                    
                    exchange.Listings.Add(listing);
                }

                _repository.CreateExchange(exchange);
            }

            return validationResults;
        }

        public IEnumerable<ValidationResult> AddUserReview(string userName, string comment, int rating, Exchange exchange)
        {
            var review = new UserReview
            {
                UserName = userName,
                Comment = comment,
                Rating = rating,
                DatePosted = DateTime.Now,
                Exchange = exchange
            };
            var validationResults = ValidateByTryValidateObject(review);

            if (!validationResults.Any())
            {
                exchange.Reviews.Add(review);

                _repository.CreateUserReview(review);
            }

            return validationResults;
        }

        public IEnumerable<UserReview> GetAllUserReviews()
        {
            return _repository.ReadAllUserReviews();
        }
        
        public void AddListing(int exchangeId, int cryptoId, DateTime listingDate)
        {
            var exchange = _repository.ReadExchange(exchangeId);
            if (exchange == null)
            {
                throw new ArgumentException("Exchange not found.", nameof(exchangeId));
            }

            var crypto = _repository.ReadCryptocurrency(cryptoId);
            if (crypto == null)
            {
                throw new ArgumentException("Cryptocurrency not found.", nameof(cryptoId));
            }

            var listing = new ExchangeListing
            {
                ExchangeId = exchangeId,
                CryptocurrencyId = cryptoId,
                ListingDate = listingDate
            };

            _repository.AddListing(listing);
        }

        public void RemoveListing(int exchangeId, int cryptoId)
        {
            _repository.RemoveListing(exchangeId, cryptoId);
        }
        
        private static List<ValidationResult> ValidateByTryValidateObject(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
