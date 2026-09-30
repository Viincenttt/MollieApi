using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models {
    /// <summary>
    /// The amount of a payment, refund, or chargeback.
    /// </summary>
    [JsonConverter(typeof(AmountJsonConverter))]
    public record Amount {
        private const int DefaultNumberOfDecimals = 2;

        private static readonly Dictionary<string, int> CurrenciesWithAlternativeNumberOfDecimals =
            new(StringComparer.OrdinalIgnoreCase) {
                { Models.Currency.JPY, 0 },
                { Models.Currency.ISK, 0 }
            };

        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// The exact monetary amount in the given currency. The value is serialized as a string with the number of
        /// decimals of the currency, for example "20.00" for EUR and "20" for JPY. Only zeros are added or removed,
        /// the value is never rounded.
        /// </summary>
        public required decimal Value { get; set; }

        /// <summary>
        /// Constructor for constructing based on a decimal value
        /// </summary>
        /// <param name="currency">An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.</param>
        /// <param name="value">The amount in the specified currency.</param>
        [SetsRequiredMembers]
        public Amount(string currency, decimal value) {
            Currency = currency;
            Value = value;
        }

        /// <summary>
        /// Constructor used by the JSON serializer
        /// </summary>
        [JsonConstructor]
        public Amount() {
        }

        /// <summary>
        /// Implicit cast operator from Amount to decimal.
        /// </summary>
        /// <param name="amount"></param>
        public static implicit operator decimal(Amount amount) => amount.Value;

        /// <summary>
        /// Implicit cast operator from Amount? to decimal?.
        /// </summary>
        /// <param name="amount"></param>
        public static implicit operator decimal?(Amount? amount) => amount?.Value;

        /// <summary>
        /// Formats the value the way it is sent to Mollie: with the number of decimals of the currency, by adding or
        /// removing zeros. A value that has more significant decimals than the currency allows is returned as is, so
        /// it is never silently rounded.
        /// </summary>
        internal string ToFormattedValue() {
            if (Currency == null || !CurrenciesWithAlternativeNumberOfDecimals.TryGetValue(Currency, out int numberOfDecimals)) {
                numberOfDecimals = DefaultNumberOfDecimals;
            }

            if (decimal.Round(Value, numberOfDecimals) != Value) {
                return Value.ToString(CultureInfo.InvariantCulture);
            }

            return Value.ToString("F" + numberOfDecimals, CultureInfo.InvariantCulture);
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
