using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    /// <summary>
    /// Used for payment methods that do not have a dedicated response type. The method specific details are
    /// kept as raw JSON, so they are not lost.
    /// </summary>
    public record GenericPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details, as raw JSON. Use <see cref="GetDetails{T}"/> to deserialize it into your own type.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Details { get; set; }

        public T? GetDetails<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
            return Details != null ? JsonSerializer.Deserialize<T>(Details, jsonSerializerOptions) : default;
        }
    }
}
