using CryptoMonitor.BL;
using CryptoMonitor.Domain;
using CryptoMonitor.DAL;

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
            // Initialize repository and manager
            IRepository repository = new InMemoryRepository();
            IManager manager = new CryptoManager(repository);

            // Seed the data
            InMemoryRepository.Seed();
            
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
            var cryptos = _manager.GetAllCryptocurrencies();
            foreach (var crypto in cryptos)
            {
                Console.WriteLine(crypto);
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
            var exchanges = _manager.GetAllExchanges();
            foreach (var exchange in exchanges)
            {
                Console.WriteLine(exchange);
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
            double.TryParse(Console.ReadLine(), out double price);

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

            _manager.AddCryptocurrency(name, symbol, price, type, maxSupply, selectedExchanges);
        }

        private void AddExchange()
        {
            Console.Write("Enter Exchange Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Exchange Website: ");
            string website = Console.ReadLine();

            Console.Write("Enter Trust Score: ");
            int trustScore = int.Parse(Console.ReadLine());

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
            
            _manager.AddExchange(name, website, trustScore, selectedCryptocurrencies);
        }
        
        private void AddUserReview()
        {
            Console.WriteLine("--- Add New Review ---");
    
            // 1. Get Review Details
            Console.Write("Enter User Name: ");
            string userName = Console.ReadLine();

            Console.Write("Enter Comment: ");
            string comment = Console.ReadLine();

            Console.Write("Enter Rating (0-5): ");
            if (int.TryParse(Console.ReadLine(), out int rating))
            {
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

                _manager.AddUserReview(userName, comment, rating, selectedExchange);
            }
            else
            {
                Console.WriteLine("Invalid exchange selection.");
            }
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
            Console.Write("Choice (0 - 7): ");
        }
    }
}