using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models {
    /// <summary>
    /// The amount of a payment, refund, or chargeback.
    /// </summary>
    public record Amount {
        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// The exact monetary amount in the given currency. The value is serialized as a string to ensure the
        /// correct number of decimals are passed, preserving the exact value set by the user.
        /// </summary>
        [JsonConverter(typeof(DecimalToStringConverter))]
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
