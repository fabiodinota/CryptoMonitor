using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptoMonitor.UI.MVC.Models
{
    public class AddExchangeListingViewModel
    {
        [Required(ErrorMessage = "Selecteer een cryptocurrency")]
        [Display(Name = "Cryptocurrency")]
        public int SelectedCryptoId { get; set; }

        [Required(ErrorMessage = "Selecteer een exchange")]
        [Display(Name = "Exchange")]
        public int SelectedExchangeId { get; set; }

        // Data voor de dropdowns
        public SelectList? Cryptos { get; set; }
        public SelectList? Exchanges { get; set; }
    }
}