using CryptoMonitor.BL;
using CryptoMonitor.Domain;
using CryptoMonitor.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptoMonitor.Controllers;

public class CryptoController : Controller
{
    private readonly IManager _cryptoManager;

    public CryptoController(IManager cryptoManager)
    {
        _cryptoManager = cryptoManager;
    }
    
    public IActionResult Index()
    {
        var cryptos = _cryptoManager.GetAllCryptocurrencies();
        return View(cryptos);
    }

    [HttpGet]
    public IActionResult Add()
    {
        var allExchanges = _cryptoManager.GetAllExchanges();

        var model = new AddCryptocurrencyViewModel
        {
            AvailableExchanges = new SelectList(allExchanges, "Id", "Name")
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(AddCryptocurrencyViewModel model)
    {
        if (ModelState.IsValid)
        {
            var allExchanges = _cryptoManager.GetAllExchanges();
            var selectedExchanges = allExchanges
                .Where(e => model.SelectedExchangeIds.Contains(e.Id))
                .ToList();
            
            try
            {
                _cryptoManager.AddCryptocurrency(
                    model.Name, 
                    model.Symbol, 
                    model.CurrentPrice, 
                    model.Type, 
                    model.MaxSupply, 
                    selectedExchanges
                );

                return RedirectToAction("Index", "Crypto");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Fout bij opslaan: " + ex.Message);
            }
        }
        
        model.AvailableExchanges = new SelectList(_cryptoManager.GetAllExchanges(), "Id", "Name");
        return View(model);
    }
    
    public IActionResult Details(int id)
    {
        
        var crypto = _cryptoManager.GetCryptocurrencyWithExchanges(id);
        
        if (crypto == null)
        {
            return NotFound();
        }
        
        return View(crypto);
    }
}