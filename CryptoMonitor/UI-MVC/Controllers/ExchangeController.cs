using CryptoMonitor.BL;
using CryptoMonitor.Domain;
using CryptoMonitor.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptoMonitor.Controllers;

public class ExchangeController : Controller
{
    private readonly IManager _cryptoManager;

    public ExchangeController(IManager cryptoManager)
    {
        _cryptoManager = cryptoManager;
    }
    
    public IActionResult Index()
    {
        // Haal alle exchanges op via de Manager
        var exchanges = _cryptoManager.GetAllExchanges();
        return View(exchanges);
    }

    [HttpGet]
    public IActionResult Add()
    {
        var allCryptos = _cryptoManager.GetAllCryptocurrencies();

        var model = new AddExchangeViewModel
        {
            AvailableCryptos = new SelectList(allCryptos, "Id", "Name") 
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(AddExchangeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var allCryptos = _cryptoManager.GetAllCryptocurrencies();
            var selectedCryptos = allCryptos
                .Where(c => model.SelectedCryptoIds.Contains(c.Id))
                .ToList();

            var validationErrors = _cryptoManager.AddExchange(
                model.Name, 
                model.Website ?? "",
                model.TrustScore, 
                selectedCryptos
            );

            if (!validationErrors.Any())
            {
                return RedirectToAction("Index", "Exchange");
            }

            foreach (var error in validationErrors)
            {
                var key = error.MemberNames?.FirstOrDefault() ?? string.Empty;
                ModelState.AddModelError(key, error.ErrorMessage ?? "Validation failed.");
            }
        }

        model.AvailableCryptos = new SelectList(_cryptoManager.GetAllCryptocurrencies(), "Id", "Name");
        return View(model);
    }
    
    public IActionResult Details(int id)
    {

        var exchange = _cryptoManager.GetExchangeWithDetails(id);
            
        if (exchange == null)
        {
            return NotFound();
        }
        
        return View(exchange);
    }
}
