using CryptoMonitor.BL;
using CryptoMonitor.Domain;

namespace CryptoMonitor.UI.CA
{
    class Program
    {
        // comp root
        private static CryptoManager _manager = new CryptoManager();

        static void Main(string[] args)
        {
            // Seed the data
            _manager.Seed();
            Console.WriteLine("Data Seeded.");

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
                        ShowCryptosByGenre();
                        break;
                    case "3":
                        ShowAllExchanges();
                        break;
                    case "4":
                        ShowExchangesFiltered();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                
                if(running) 
                {
                    Console.WriteLine("\nPress ENTER to continue...");
                    Console.ReadLine();
                }
            }
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Crypto Monitor - Sprint 1");
            Console.WriteLine("=========================");
            Console.WriteLine("0) Quit");
            Console.WriteLine("1) Show all Cryptos");
            Console.WriteLine("2) Show Cryptos by Type (Mandatory)");
            Console.WriteLine("3) Show all Exchanges");
            Console.WriteLine("4) Show Exchanges with Name and/or Min Trust (Optional)");
            Console.Write("Choice: ");
        }

        private static void ShowAllCryptos()
        {
            foreach (var c in _manager.GetAllCryptos())
            {
                Console.WriteLine(c.ToString());
            }
        }

        private static void ShowCryptosByGenre()
        {
            Console.WriteLine("Select Type:");
            foreach (var typeName in Enum.GetNames(typeof(CryptoType)))
            {
                Console.WriteLine($"- {typeName}");
            }
            
            Console.Write("Enter Type: ");
            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out CryptoType selectedType))
            {
                var results = _manager.GetCryptosByType(selectedType);
                Console.WriteLine($"\nResults for {selectedType}:");
                foreach (var c in results)
                {
                    Console.WriteLine(c.ToString());
                }
            }
            else
            {
                Console.WriteLine("Invalid Type.");
            }
        }

        private static void ShowAllExchanges()
        {
            foreach (var e in _manager.GetAllExchanges())
            {
                Console.WriteLine(e.ToString());
                // Show nested reviews just to prove relations work
                foreach(var r in e.Reviews)
                {
                    Console.WriteLine($"  -> Review: {r.Rating}/5: {r.Comment}");
                }
            }
        }

        private static void ShowExchangesFiltered()
        {
            Console.WriteLine("Optional filters (leave blank to skip):");
            
            Console.Write("Enter (part of) Name: ");
            string nameInput = Console.ReadLine();

            Console.Write("Enter Minimum Trust Score (1-10): ");
            string scoreInput = Console.ReadLine();
            int? score = null;
            if (int.TryParse(scoreInput, out int parsedScore))
            {
                score = parsedScore;
            }

            var results = _manager.GetExchangesFiltered(nameInput, score);
            
            Console.WriteLine("\nFiltered Exchanges:");
            bool found = false;
            foreach (var e in results)
            {
                Console.WriteLine(e.ToString());
                found = true;
            }
            if (!found) Console.WriteLine("No exchanges match your criteria.");
        }
    }
}