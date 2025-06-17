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
        /// Can be updated while the payment is in an open state.
        /// </summary>
        public string? CancelUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send payment status updates to.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the payment. Whenever
        /// you fetch the payment with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// Can be updated while no payment method has been chosen yet.
        /// See the Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// Allows you to preset the language to be used in the payment screens shown to the consumer. Setting a locale is highly
        /// recommended and will greatly improve your conversion rate. When this parameter is omitted, the browser language will
        /// be used instead if supported by the payment method. You can provide any ISO 15897 locale, but our payment screen currently
        /// only supports the following languages: en_US nl_NL nl_BE fr_FR fr_BE de_DE de_AT de_CH es_ES ca_ES pt_PT it_IT nb_NO
        /// sv_SE fi_FI da_DK is_IS hu_HU pl_PL lv_LV lt_LT
        /// </summary>
        public string? Locale { get; set; }

        /// <summary>
        /// For digital goods in most jurisdictions, you must apply the VAT rate from your customer’s country. Choose the VAT rates
        /// you have used for the order to ensure your customer’s country matches the VAT country. Use this parameter to restrict the
        /// payment methods available to your customer to those from a single country. If available, the credit card method will still
        /// be offered, but only cards from the allowed country are accepted.
        /// </summary>
        public string? RestrictPaymentMethodsToCountry { get; set; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this payment a test payment.
        /// </summary>
        public bool? Testmode { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}
