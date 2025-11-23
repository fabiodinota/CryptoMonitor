using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CryptoMonitor.Domain
{
    public class Cryptocurrency : IValidatableObject
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; }
        
        [Required]
        public double CurrentPrice { get; set; }
        
        [Required]
        public CryptoType Type { get; set; }
        
        [Required]
        public long? MaxSupply { get; set; }

        // nav props (*-*)
        public List<Exchange> Exchanges { get; set; } = new List<Exchange>();
        
        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (this.Name == this.Symbol)
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