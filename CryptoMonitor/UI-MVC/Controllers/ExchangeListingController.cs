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

      
        [HttpGet]
        public IActionResult Index()
        {
            
            var exchanges = _cryptoManager.GetAllExchangesWithDetails();
            
            var allListings = exchanges
                .SelectMany(e => e.Listings)
                .OrderByDescending(l => l.ListingDate)
                .ToList();

            return View(allListings);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddExchangeListingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _cryptoManager.AddListing(model.SelectedExchangeId, model.SelectedCryptoId, DateTime.Now);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Fout bij toevoegen: " + ex.Message);
                }
            }

            model.Cryptos = new SelectList(_cryptoManager.GetAllCryptocurrencies(), "Id", "Name");
            model.Exchanges = new SelectList(_cryptoManager.GetAllExchanges(), "Id", "Name");
            
            return View(model);
        }
    }
}