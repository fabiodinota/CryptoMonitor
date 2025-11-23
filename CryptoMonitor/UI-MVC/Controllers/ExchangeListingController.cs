using CryptoMonitor.BL;
using CryptoMonitor.Domain;
using CryptoMonitor.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptoMonitor.UI.MVC.Controllers
{
    public class ExchangeListingController : Controller
    {
        private readonly IManager _cryptoManager;

        public ExchangeListingController(IManager manager)
        {
            _cryptoManager = manager;
        }

        // GET: /ExchangeListing
        // Toont een overzicht van alle listings ("New exchange listings")
        [HttpGet]
        public IActionResult Index()
        {
            // We halen alle exchanges op met hun listings (Eager Loading)
            // en 'flatten' dit naar één grote lijst van listings.
            var exchanges = _cryptoManager.GetAllExchangesWithDetails();
            
            var allListings = exchanges
                .SelectMany(e => e.Listings)
                .OrderByDescending(l => l.ListingDate) // Nieuwste eerst
                .ToList();

            return View(allListings);
        }

        // GET: /ExchangeListing/Add
        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddExchangeListingViewModel
            {
                Cryptos = new SelectList(_cryptoManager.GetAllCryptocurrencies(), "Id", "Name"),
                Exchanges = new SelectList(_cryptoManager.GetAllExchanges(), "Id", "Name")
            };

            return View(model);
        }

        // POST: /ExchangeListing/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddExchangeListingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Roep de methode aan die we in Sprint 4 hebben gemaakt
                    _cryptoManager.AddListing(model.SelectedExchangeId, model.SelectedCryptoId);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Fout bij toevoegen: " + ex.Message);
                }
            }

            // Als het mislukt, vul de dropdowns opnieuw
            model.Cryptos = new SelectList(_cryptoManager.GetAllCryptocurrencies(), "Id", "Name");
            model.Exchanges = new SelectList(_cryptoManager.GetAllExchanges(), "Id", "Name");
            
            return View(model);
        }
    }
}