using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CryptoMonitor.Models;
using CryptoMonitor.BL;
using UI_MVC.Models;

namespace CryptoMonitor.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IManager _manager;

    public HomeController(ILogger<HomeController> logger, IManager manager)
    {
        _logger = logger;
        _manager = manager;
    }

    public IActionResult Index()
    {
        var allCryptos = _manager.GetAllCryptocurrencies();
        var allExchanges = _manager.GetAllExchanges();
        var allReviews = _manager.GetAllUserReviews();

        var viewModel = new DashboardViewModel
        {
            TotalCryptos = allCryptos.Count(),
            TotalExchanges = allExchanges.Count(),
            TotalReviews = allReviews.Count(),
            RecentCryptos = allCryptos.OrderByDescending(c => c.Id).Take(5).ToList(),
            RecentReviews = allReviews.OrderByDescending(r => r.DatePosted).Take(5).ToList()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}