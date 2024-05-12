using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Mollie.WebApplication.Blazor.Framework.Validators;

public class StaticStringListAttribute : ValidationAttribute {
    private readonly Type _staticClass;

    public StaticStringListAttribute(Type staticClass) {
        _staticClass = staticClass;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        IEnumerable<string> validValues = _staticClass
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(x => x.GetValue(null).ToString());

        if (validValues.Contains(value)) {
            return ValidationResult.Success;
        }

        return new ValidationResult($"The value \"{value}\" is invalid");
    }
}
