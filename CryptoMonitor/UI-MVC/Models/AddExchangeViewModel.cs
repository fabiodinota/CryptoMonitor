using System.ComponentModel.DataAnnotations;
using CryptoMonitor.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptoMonitor.UI.MVC.Models
{
    public class AddExchangeViewModel
    {
        [Required(ErrorMessage = "Naam is verplicht")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Website { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Trust Score moet tussen 1 en 10 liggen")]
        public int TrustScore { get; set; }

        // De lijst van Crypto ID's die de gebruiker selecteert om te koppelen
        [Display(Name = "Genoteerde Cryptocurrencies")]
        public List<int> SelectedCryptoIds { get; set; } = new List<int>();

        // De lijst van ALLE cryptos om de dropdown te vullen
        public SelectList? AvailableCryptos { get; set; }
    }
}