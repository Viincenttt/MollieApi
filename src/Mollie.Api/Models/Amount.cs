using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models {
    /// <summary>
    /// The amount of a payment, refund, or chargeback.
    /// </summary>
    public record Amount {
        private static readonly IDictionary<string, string> CurrenciesWithAlternativeDecimalPrecision =
            new Dictionary<string, string>() {
                { "JPY", "0" },
                { "ISK", "0" },
            };

        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// Constructor for constructing based on a string value
        /// </summary>
        /// <param name="currency">An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.</param>
        /// <param name="value">A string containing an exact monetary amount in the given currency.</param>
        [JsonConstructor]
        [SetsRequiredMembers]
        public Amount(string currency, string value) {
            Currency = currency;
            Value = value;
        }

        /// <summary>
        /// Constructor for constructing based on a decimal value
        /// </summary>
        /// <param name="currency">An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.</param>
        /// <param name="value">The amount in the specified currency.</param>
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
            => decimal.TryParse(amount.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var a) ? a : throw new InvalidCastException($"Cannot convert {amount.Value} to decimal");

        /// <summary>
        /// Implicit cast operator from Amount? to decimal?.
        /// </summary>
        /// <param name="amount"></param>
        public static implicit operator decimal?(Amount? amount)
            => amount == null ? null : (decimal)amount;

        private static string ConvertDecimalAmountToStringAmount(string currency, decimal value) {
            if (CurrenciesWithAlternativeDecimalPrecision.TryGetValue(currency, out string? format)) {
                return value.ToString(format, CultureInfo.InvariantCulture);
            }

            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        public override string ToString() {
            return $"{Value} {Currency}";
        }

        public override int GetHashCode() {
            unchecked {
                return (Currency.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }
    }
}
