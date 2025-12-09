using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMonitor.Domain
{
    public class Cryptocurrency : IValidatableObject
    {
        public int Id { get; set; }
        [Required, MaxLength(50)] public string Name { get; set; } = string.Empty;
        
        [Required, MaxLength(10)] public string Symbol { get; set; } = string.Empty;

        [Required, Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price must be greater than 0")]
        public decimal CurrentPrice { get; set; }

        [Required] public CryptoType Type { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Max supply must be positive")]
        public long? MaxSupply { get; set; }
        
        public List<ExchangeListing> Listings { get; set; } = new List<ExchangeListing>();
        
        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            string normalizedName = (this.Name ?? string.Empty).Trim().ToLowerInvariant();
            string normalizedSymbol = (this.Symbol ?? string.Empty).Trim().ToLowerInvariant();

            if (normalizedName == normalizedSymbol)
            {
                string errorMessage = "The Cryptocurrency's Name can't be the same as it's symbol.";
                errors.Add(new ValidationResult(errorMessage,
                    new string[] { "Name", "Symbol" }));
            }
            return errors;
        }

        public override string ToString()
        {
            string supply = 
                MaxSupply.HasValue 
                    ? MaxSupply.Value.ToString("N0") 
                    : "Infinite";
            
            return $"{Name} ({Symbol}) - €{CurrentPrice:N2} [{Type}] | Supply: {supply}";
        }
    }
}
