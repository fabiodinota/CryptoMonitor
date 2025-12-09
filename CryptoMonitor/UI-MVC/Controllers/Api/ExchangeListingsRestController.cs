using CryptoMonitor.BL;
using CryptoMonitor.Domain;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeListingsRestController : ControllerBase
    {
        private readonly IManager _manager;

        public ExchangeListingsRestController(IManager manager)
        {
            _manager = manager;
        }

        [HttpGet("by-exchange/{exchangeId}")]
        public IActionResult GetListings(int exchangeId)
        {
            var exchange = _manager.GetExchangeWithDetails(exchangeId);
            
            if (exchange == null)
            {
                return NotFound($"Exchange with ID {exchangeId} not found.");
            }

            var listings = exchange.Listings.Select(l => new 
            {
                l.CryptocurrencyId,
                CryptocurrencyName = l.Cryptocurrency?.Name ?? "Unknown",
                CryptocurrencySymbol = l.Cryptocurrency?.Symbol ?? "???",
                l.ListingDate
            });

            return Ok(listings);
        }

        [HttpGet("available-cryptos/{exchangeId}")]
        public IActionResult GetAvailableCryptos(int exchangeId)
        {
            var exchange = _manager.GetExchangeWithDetails(exchangeId);
            if (exchange == null)
            {
                return NotFound($"Exchange with ID {exchangeId} not found.");
            }

            var allCryptos = _manager.GetAllCryptocurrencies();
            
            // Filter out already listed cryptos
            var listedCryptoIds = exchange.Listings.Select(l => l.CryptocurrencyId).ToHashSet();
            
            var availableParams = allCryptos
                .Where(c => !listedCryptoIds.Contains(c.Id))
                .Select(c => new 
                {
                    c.Id,
                    c.Name,
                    c.Symbol
                });

            return Ok(availableParams);
        }

        [HttpPost]
        public IActionResult AddListing([FromBody] CreateListingDto dto)
        {
             if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _manager.AddListing(dto.ExchangeId, dto.CryptoId, dto.ListingDate);
                return Ok();
            }
             catch (ArgumentException ex)
             {
                 return BadRequest(ex.Message);
             }
             catch (InvalidOperationException ex)
             {
                 return Conflict(ex.Message);
             }
        }
    }

    public class CreateListingDto
    {
        public int ExchangeId { get; set; }
        public int CryptoId { get; set; }
        public DateTime ListingDate { get; set; }
    }
}
