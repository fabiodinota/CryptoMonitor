using CryptoMonitor.BL;
using CryptoMonitor.Domain;
using CryptoMonitor.DAL;
using CryptoMonitor.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CryptoMonitor.UI.CA
{
    class Program
    {
        private readonly IManager _manager;


        public Program(IManager manager)
        {
            _manager = manager;
        }

        static void Main(string[] args)
        {
            string dbPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../CryptoMonitor.db"));
    
            Console.WriteLine($"Database bestand locatie: {dbPath}");

            var optionsBuilder = new DbContextOptionsBuilder<CryptoMonitorDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            using var context = new CryptoMonitorDbContext(optionsBuilder.Options);         
            
            bool dbCreated = context.CreateDatabase(false); 
            
            if (dbCreated)
            {
                Console.WriteLine("Database created successfully.");
                DataSeeder.Seed(context); 
                Console.WriteLine("Data seeded.");
            }
            else
            {
                Console.WriteLine("Database already exists. Seeding skipped.");
            }
            
            IRepository repository = new Repository(context);

            IManager manager = new CryptoManager(repository);
            
            Program program = new Program(manager);
            program.Run();
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "0":
                        running = false;
                        break;
                    case "1":
                        ShowAllCryptos();
                        break;
                    case "2":
                        ShowCryptosByType();
                        break;
                    case "3":
                        ShowAllExchanges();
                        break;
                    case "4":
                        ShowExchangesFiltered();
                        break;
                    case "5":
                        AddCryptocurrency();
                        break;
                    case "6":
                        AddExchange();
                        break;
                    case "7":
                        AddUserReview();
                        break;
                    case "8": 
                        AddCryptoToExchange(); 
                        break;
                    case "9": 
                        RemoveCryptoFromExchange(); 
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress ENTER to continue...");
                    Console.ReadLine();
                }
            }
        }

        private void ShowAllCryptos()
        {
            Console.WriteLine("--- All Cryptos (incl. Exchanges) ---");
            var cryptos = _manager.GetAllCryptocurrenciesWithExchanges();
            
            foreach (var crypto in cryptos)
            {
                Console.WriteLine(crypto.ToString());
                
                if (crypto.Listings != null && crypto.Listings.Any())
                {
                    foreach (var listing in crypto.Listings)
                    {
                        if (listing.Exchange != null)
                        {
                            Console.WriteLine($"   -> Available at: {listing.Exchange.Name} (listed on {listing.ListingDate:F})");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("   -> Not yet listed on an exchange.");
                }
                Console.WriteLine();
            }
        }

        private void ShowCryptosByType()
        {
            Console.Write("Enter Crypto Type (Coin, Token, Stablecoin, MemeCoin): ");
            string typeStr = Console.ReadLine();
            if (Enum.TryParse<CryptoType>(typeStr, true, out var type))
            {
                var cryptos = _manager.GetCryptocurrenciesFiltered(type, null);
                foreach (var crypto in cryptos)
                {
                    Console.WriteLine(crypto);
                }
            }
            else
            {
                Console.WriteLine("Invalid crypto type.");
            }
        }

        private void ShowAllExchanges()
        {
            Console.WriteLine("--- All Exchanges (incl. Cryptocurrencies & Reviews) ---");
            var exchanges = _manager.GetAllExchangesWithDetails();

            foreach (var exchange in exchanges)
            {
                Console.WriteLine(exchange.ToString());

                Console.WriteLine("   [Cryptocurrencies]:");
                if (exchange.Listings != null && exchange.Listings.Any())
                {
                    foreach (var listing in exchange.Listings)
                    {
                        Console.WriteLine($"     - ${listing.Cryptocurrency?.Symbol} ({listing.Cryptocurrency?.Name})");
                    }
                }
                else
                {
                    Console.WriteLine("     - no cryptocurrencies available.");
                }

                // 2. Toon Reviews
                Console.WriteLine("   [Reviews]:");
                if (exchange.Reviews != null && exchange.Reviews.Any())
                {
                    foreach (var review in exchange.Reviews)
                    {
                        Console.WriteLine($"     - {review.Rating}/5 by {review.UserName}: \"{review.Comment}\"");
                    }
                }
                else
                {
                    Console.WriteLine("     - no reviews.");
                }
                Console.WriteLine("-------------------------------");
            }
        }

        private void ShowExchangesFiltered()
        {
            Console.Write("Enter part of the exchange name (or leave blank): ");
            string namePart = Console.ReadLine();

            Console.Write("Enter minimum trust score (or leave blank): ");
            string scoreStr = Console.ReadLine();
            int? minTrustScore = null;
            if (int.TryParse(scoreStr, out var score))
            {
                minTrustScore = score;
            }

            var exchanges = _manager.GetExchangesFiltered(namePart, minTrustScore);
            foreach (var exchange in exchanges)
            {
                Console.WriteLine(exchange);
            }
        }

        private void AddCryptocurrency()
        {
            Console.Write("Enter Crypto Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Crypto Symbol: ");
            string symbol = Console.ReadLine();

            Console.Write("Enter Current Price: ");
            decimal.TryParse(Console.ReadLine(), out decimal price);

            Console.WriteLine("Available Crypto Types:");
            foreach (var typeName in Enum.GetNames(typeof(CryptoType)))
            {
                Console.WriteLine(typeName);
            }
            Console.Write("Enter Crypto Type: ");
            Enum.TryParse<CryptoType>(Console.ReadLine(), true, out var type);

            Console.Write("Enter Max Supply (or leave blank): ");
            long? maxSupply = null;
            if (long.TryParse(Console.ReadLine(), out var supply))
            {
                maxSupply = supply;
            }

            var allExchanges = _manager.GetAllExchanges().ToList();
            var selectedExchanges = new List<Exchange>();
            Console.WriteLine("Available Exchanges:");
            for (int i = 0; i < allExchanges.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {allExchanges[i].Name}");
            }
            Console.Write("Enter comma-separated numbers of exchanges to link: ");
            var exchangeChoices = Console.ReadLine().Split(',');
            foreach (var choice in exchangeChoices)
            {
                if (int.TryParse(choice.Trim(), out int index) && index > 0 && index <= allExchanges.Count)
                {
                    selectedExchanges.Add(allExchanges[index - 1]);
                }
            }

            var validationErrors = _manager.AddCryptocurrency(name, symbol, price, type, maxSupply, selectedExchanges);
            if (validationErrors.Any())
            {
                Console.WriteLine("Validation failed:");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
            }
            else
            {
                Console.WriteLine("Cryptocurrency added successfully!");
            }
        }

        private void AddExchange()
        {
            Console.Write("Enter Exchange Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Exchange Website: ");
            string website = Console.ReadLine();

            Console.Write("Enter Trust Score: ");
            int trustScore;
            string? trustInput = Console.ReadLine();
            while (!int.TryParse(trustInput, out trustScore))
            {
                Console.Write("Invalid input. Enter a numeric Trust Score: ");
                trustInput = Console.ReadLine();
            }

            var allCryptocurrencies = _manager.GetAllCryptocurrencies().ToList();
            var selectedCryptocurrencies = new List<Cryptocurrency>();
            Console.WriteLine("Available Cryptocurrencies:");
            for (int i = 0; i < allCryptocurrencies.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {allCryptocurrencies[i].Name}");
            }
            Console.Write("Enter comma-separated numbers of cryptocurrencies to link: ");
            var cryptoChoices = Console.ReadLine().Split(',');
            foreach (var choice in cryptoChoices)
            {
                if (int.TryParse(choice.Trim(), out int index) && index > 0 && index <= allCryptocurrencies.Count)
                {
                    selectedCryptocurrencies.Add(allCryptocurrencies[index - 1]);
                }
            }
            
            var validationErrors = _manager.AddExchange(name, website, trustScore, selectedCryptocurrencies);
            if (validationErrors.Any())
            {
                Console.WriteLine("Validation failed:");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
            }
            else
            {
                Console.WriteLine("Exchange added successfully!");
            }
        }
        
        private void AddUserReview()
        {
            Console.WriteLine("--- Add New Review ---");
    
            Console.Write("Enter User Name: ");
            string userName = Console.ReadLine();

            Console.Write("Enter Comment: ");
            string comment = Console.ReadLine();

            Console.Write("Enter Rating (0-5): ");

            int rating;
            string? ratingInput = Console.ReadLine();
            if (!int.TryParse(ratingInput, out rating) || rating < 0 || rating> 5)
            {
                Console.WriteLine("Invalid rating. Defaulting to 0.");
                rating = 0;
            }

            var allExchanges = _manager.GetAllExchanges().ToList();
            if (allExchanges.Count == 0)
            {
                Console.WriteLine("No exchanges available to review. Create an exchange first.");
                return;
            }

            Console.WriteLine("\nSelect an Exchange to review:");
            for (int i = 0; i < allExchanges.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {allExchanges[i].Name}");
            }

            Console.Write("Enter number: ");
            string choiceStr = Console.ReadLine();

            if (int.TryParse(choiceStr, out int index) && index > 0 && index <= allExchanges.Count)
            {
                var selectedExchange = allExchanges[index - 1];

                var validationErrors = _manager.AddUserReview(userName, comment, rating, selectedExchange);
                if (validationErrors.Any())
                {
                    Console.WriteLine("Validation failed:");
                    foreach (var error in validationErrors)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.WriteLine("User Review added successfully!");
                }
            }
            else
            {
                Console.WriteLine("Invalid exchange selection.");
            }
        }
        
        private void AddCryptoToExchange()
        {
            Console.WriteLine("--- Koppel Crypto aan Exchange ---");
            
            var exchanges = _manager.GetAllExchanges().ToList();
            if (!exchanges.Any()) return;

            Console.WriteLine("Selecteer een Exchange:");
            for (int i = 0; i < exchanges.Count; i++)
                Console.WriteLine($"[{i + 1}] {exchanges[i].Name}");

            Console.Write("Keuze: ");
            if (!int.TryParse(Console.ReadLine(), out int exIndex) || exIndex < 1 || exIndex > exchanges.Count)
            {
                Console.WriteLine("Ongeldige keuze.");
                return;
            }
            var selectedExchange = exchanges[exIndex - 1];

            var cryptos = _manager.GetAllCryptocurrencies().ToList();
            if (!cryptos.Any()) return;

            Console.WriteLine("\nSelecteer een Cryptocurrency om toe te voegen:");
            for (int i = 0; i < cryptos.Count; i++)
                Console.WriteLine($"[{i + 1}] {cryptos[i].Symbol} ({cryptos[i].Name})");

            Console.Write("Keuze: ");
            if (!int.TryParse(Console.ReadLine(), out int cryptoIndex) || cryptoIndex < 1 || cryptoIndex > cryptos.Count)
            {
                Console.WriteLine("Ongeldige keuze.");
                return;
            }
            var selectedCrypto = cryptos[cryptoIndex - 1];

            try 
            {
                _manager.AddListing(selectedExchange.Id, selectedCrypto.Id, DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij toevoegen (bestaat de relatie al?): {ex.Message}");
            }
        }

        private void RemoveCryptoFromExchange()
        {
            Console.WriteLine("--- Verwijder Crypto van Exchange ---");
            
            var exchanges = _manager.GetAllExchangesWithDetails().ToList(); 
            if (!exchanges.Any()) return;

            Console.WriteLine("Selecteer een Exchange:");
            for (int i = 0; i < exchanges.Count; i++)
                Console.WriteLine($"[{i + 1}] {exchanges[i].Name}");

            Console.Write("Keuze: ");
            if (!int.TryParse(Console.ReadLine(), out int exIndex) || exIndex < 1 || exIndex > exchanges.Count) return;
            
            var selectedExchange = exchanges[exIndex - 1];

            if (selectedExchange.Listings == null || !selectedExchange.Listings.Any())
            {
                Console.WriteLine("Deze exchange heeft geen cryptos om te verwijderen.");
                return;
            }

            Console.WriteLine($"\nSelecteer een Cryptocurrency om te verwijderen van {selectedExchange.Name}:");
            var listings = selectedExchange.Listings.ToList();
            
            for (int i = 0; i < listings.Count; i++)
            {
                var symbol = listings[i].Cryptocurrency?.Symbol ?? $"ID: {listings[i].CryptocurrencyId}";
                Console.WriteLine($"[{i + 1}] {symbol}");
            }

            Console.Write("Keuze: ");
            if (!int.TryParse(Console.ReadLine(), out int listIndex) || listIndex < 1 || listIndex > listings.Count)
            {
                Console.WriteLine("Ongeldige keuze.");
                return;
            }

            var listingToRemove = listings[listIndex - 1];

            _manager.RemoveListing(listingToRemove.ExchangeId, listingToRemove.CryptocurrencyId);
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Crypto Monitor");
            Console.WriteLine("=========================");
            Console.WriteLine("0) Quit");
            Console.WriteLine("1) Show all Cryptos");
            Console.WriteLine("2) Show Cryptos by Type");
            Console.WriteLine("3) Show all Exchanges");
            Console.WriteLine("4) Show Exchanges with Name and/or Min Trust");
            Console.WriteLine("5) Add Crypto");
            Console.WriteLine("6) Add Exchange");
            Console.WriteLine("7) Add User Review");
            Console.WriteLine("8) Koppel Crypto aan Exchange (+)");
            Console.WriteLine("9) Verwijder Crypto van Exchange (-)");
            Console.Write("Choice (0 - 9): ");
        }
    }
}
