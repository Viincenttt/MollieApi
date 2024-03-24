using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json;

namespace Mollie.Api.Models {
    public class Amount {
        private static readonly IDictionary<string, string> CurrenciesWithAlternativeDecimalPrecision =
            new Dictionary<string, string>() {
                { "JPY", "0" },
                { "ISK", "0" },
            };

        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public required string Currency { get; init; }

        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public required string Value { get; init; }

        [JsonConstructor]
        [SetsRequiredMembers]
        public Amount(string currency, string value) {
            Currency = currency;
            Value = value;
        }

        /// <summary>
        /// Constructor for constructing based on a decimal value
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="value"></param>
        [SetsRequiredMembers]
        public Amount(string currency, decimal value) {
            Currency = currency;
            Value = ConvertDecimalAmountToStringAmount(currency, value);
        }

        /// <summary>
        /// Implicit cast operator from Amount to decimal.
        /// </summary>
        /// <param name="amount"></param>
        public static implicit operator decimal(Amount amount)
            => Decimal.TryParse(amount.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var a) ? a : throw new InvalidCastException($"Cannot convert {amount.Value} to decimal");

        private static string ConvertDecimalAmountToStringAmount(string currency, decimal value) {
            if (CurrenciesWithAlternativeDecimalPrecision.ContainsKey(currency)) {
                string format = CurrenciesWithAlternativeDecimalPrecision[currency];
                return value.ToString(format, CultureInfo.InvariantCulture);
            }
            
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }
        
        public override string ToString() {
            return $"{Value} {Currency}";
        }

        public override bool Equals(object? obj) {
            if (obj is Amount amount) {
                return Currency == amount.Currency && Value == amount.Value;
            }

            return false;
        }

        public override int GetHashCode() {
            unchecked {
                return (Currency.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }
    }
}