using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Mollie.WebApplicationCoreExample.Models
{
    public class DecimalPlacesAttribute : ValidationAttribute
    {
        public int DecimalPlaces { get; }

        public DecimalPlacesAttribute(int decimalPlaces)
        {
            DecimalPlaces = decimalPlaces;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal amount = (decimal)value;
            string text = amount.ToString(CultureInfo.InvariantCulture);
            int dotIndex = text.IndexOf('.');
            var decimals = text.Length - dotIndex - 1;
            return dotIndex < 0 || dotIndex != text.LastIndexOf('.') || decimals != DecimalPlaces
                ? new ValidationResult(ErrorMessage ?? $"Please enter an amount with ${DecimalPlaces} decimal place(s)")
                : ValidationResult.Success;
        }
    }
}
