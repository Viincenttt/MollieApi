using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Payment.Request {
    public record PaymentUpdateRequest {
        /// <summary>
        /// The description of the payment. This will be shown to your customer on their card or bank statement 
        /// when possible. We truncate the description automatically according to the limits of the used payment
        /// method. The description is also visible in any exports you generate. We recommend you use a unique 
        /// identifier so that you can always link the payment to the order in your back office.This is particularly
        /// useful for bookkeeping.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to after the payment process. It could make sense for the redirectUrl
        /// to contain a unique identifier – like your order ID – so you can show the right page referencing the order
        /// when your customer returns. Updating this field is only possible when the payment is not yet finalized.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send payment status updates to.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the payment. Whenever 
        /// you fetch the payment with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// For digital goods in most jurisdictions, you must apply the VAT rate from your customer’s country. Choose the VAT rates 
        /// you have used for the order to ensure your customer’s country matches the VAT country. Use this parameter to restrict the 
        /// payment methods available to your customer to those from a single country. If available, the credit card method will still
        /// be offered, but only cards from the allowed country are accepted.
        /// </summary>
        public string? RestrictPaymentMethodsToCountry { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            this.Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}
