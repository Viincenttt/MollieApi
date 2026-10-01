using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record KlarnaPaymentRequest : PaymentRequest {
        public KlarnaPaymentRequest() {
            Method = PaymentMethod.KlarnaOne;
        }

        [SetsRequiredMembers]
        public KlarnaPaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.KlarnaOne;
        }

        /// <summary>
        /// For some industries, additional purchase information can be sent to Klarna to increase the authorization rate.
        /// You can submit your extra data in this field if you have agreed upon this with Klarna. This field should be a
        /// JSON object containing any of the allowed keys and sub-objects described in the Klarna developer documentation.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? ExtraMerchantData { get; set; }

        public void SetExtraMerchantData(object extraMerchantDataObj, JsonSerializerOptions? jsonSerializerOptions = null) {
            ExtraMerchantData = JsonSerializer.Serialize(extraMerchantDataObj, jsonSerializerOptions);
        }
    }
}
