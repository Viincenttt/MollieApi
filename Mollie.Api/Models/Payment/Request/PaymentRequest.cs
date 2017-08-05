using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment.Request {
    using JsonConverters;

    public class PaymentRequest {
        /// <summary>
        /// Required - The amount in EURO that you want to charge, e.g. 100.00 if you would want to charge € 100,00.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Required - The description of the payment you're creating. This will be shown to the consumer on their card or bank statement when possible.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required - The URL the consumer will be redirected to after the payment process. It could make sense for the redirectURL to contain a unique
        /// identifier – like your order ID – so you can show the right page referencing the order when the consumer returns.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Optional - Use this parameter to set a wehook URL for this payment only. Mollie will ignore any webhook set in your website profile for this 
        /// payment.
        /// </summary>
        [JsonConverter(typeof (StringEnumConverter))]
        public PaymentMethod? Method { get; set; }

        /// <summary>
        /// Optional – Provide any data you like in JSON notation, and we will save the data alongside the payment. Whenever you fetch the payment with our 
        /// API, we'll also include the metadata. You can use up to 1kB of JSON.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings jsonSerializerSettings = null) {
            this.Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }

        /// <summary>
        /// Optional - Provide any data you like in JSON notation, and we will save the data alongside the payment. Whenever you fetch the payment with our
        /// API, we'll also include the metadata. You can use up to 1kB of JSON.
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// Optional - Allow you to preset the language to be used in the payment screens shown to the consumer. When this parameter is not provided, the 
        /// browser language will be used instead (which is usually more accurate). Possible values are: de, en, es, fr, be, be-fr, nl
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Id of target customer.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Recurring type required by Mollie. First for first payment. Recurring after first successful payment.
        /// </summary>
        /// 
        public RecurringType? RecurringType { get; set; }

        /// <summary>
        /// Optional – When creating recurring payments, a specific mandate ID may be supplied to indicate which of the consumer's accounts should be credited.
        /// </summary>
        public string MandateId { get; set; }

        public override string ToString() {
            return $"Method: {this.Method} - Amount: {this.Amount}";
        }
    }
}
