using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Mollie.WebApplication.Blazor.Framework.Validators;

public class DecimalPlacesAttribute : ValidationAttribute {
    private int _decimalPlaces { get; }

    public DecimalPlacesAttribute(int decimalPlaces) {
        _decimalPlaces = decimalPlaces;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        if (value == null) {
            return new ValidationResult("Value is null");
        }

        decimal amount = (decimal)value;
        string text = amount.ToString(CultureInfo.InvariantCulture);
        int dotIndex = text.IndexOf('.');
        var decimals = text.Length - dotIndex - 1;
        var places = _decimalPlaces switch
        {
            0 => "without decimal places",
            1 => "with one decimal place",
            _ => $"with {_decimalPlaces} decimal places"
        };
        return dotIndex < 0 || dotIndex != text.LastIndexOf('.') || decimals != _decimalPlaces
            ? new ValidationResult(ErrorMessage ?? $"Please enter an amount {places}")
            : ValidationResult.Success;
    }
}
