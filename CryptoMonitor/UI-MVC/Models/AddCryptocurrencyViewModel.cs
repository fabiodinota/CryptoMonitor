using System.ComponentModel.DataAnnotations;
using CryptoMonitor.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptoMonitor.UI.MVC.Models
{
    public class AddCryptocurrencyViewModel
    {
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price has to be positive")]
        public double CurrentPrice { get; set; }

        [Required]
        public CryptoType Type { get; set; }

        public long? MaxSupply { get; set; }

        [Display(Name = "available on Exchanges")]
        public List<int> SelectedExchangeIds { get; set; } = new List<int>();

       
        public SelectList? AvailableExchanges { get; set; }
    }
}