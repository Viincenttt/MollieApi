using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Mollie.WebApplicationCoreExample.Validators {
    public class StaticStringListAttribute : ValidationAttribute {
        private readonly Type _staticClass;

        public StaticStringListAttribute(Type staticClass) {
            this._staticClass = staticClass;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            IEnumerable<string> validValues = this._staticClass
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(x => x.GetValue(null).ToString());

            if (validValues.Contains(value)) {
                return ValidationResult.Success;
            }
            
            return new ValidationResult($"The value \"{value}\" is invalid");
        }
    }
}