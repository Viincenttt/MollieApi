using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Subscription.Request {
    public record SubscriptionRequest {
        /// <summary>
        /// The constant amount in EURO that you want to charge with each subscription payment, e.g. 100.00 if you would want
        /// to charge € 100,00.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// Optional – Total number of charges for the subscription to complete. Leave empty for an on-going subscription.
        /// </summary>
        public int? Times { get; set; }

        /// <summary>
        /// Interval to wait between charges, for example 1 month or 14 days.
        /// </summary>
        public required string Interval { get; set; }

        /// <summary>
        /// Optional – The start date of the subscription in yyyy-mm-dd format. This is the first day on which your customer
        /// will be charged. When
        /// this parameter is not provided, the current date will be used instead.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// A description unique per customer. This will be included in the payment description along with the charge date in
        /// Y-m-d format.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Optional – The payment method used for this subscription, either forced on creation or null if any of the
        /// customer's valid mandates may be used. See the Mollie.Api.Models.Payment.PaymentMethod class for a full
        /// list of known values.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// The mandate used for this subscription. Please note that this parameter can not set together with method.
        /// </summary>
        public string? MandateId { get; set; }

        /// <summary>
        /// Optional – Use this parameter to set a webhook URL for all subscription payments.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Adding an application fee allows you to charge the merchant for each payment in the subscription and
        /// transfer these amounts to your own account.
        /// </summary>
        public ApplicationFee? ApplicationFee { get; set; }

        /// <summary>
        /// The website profile’s unique identifier, for example pfl_3RkSN1zuPE.
        /// </summary>
        public string? ProfileId { get; set; }

        /// <summary>
        /// Provide any data you like, and we will save the data alongside the subscription. Whenever you fetch the subscription
        /// with our API, we’ll also include the metadata. You can use up to 1kB of JSON.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this subscription a test subscription.
        /// </summary>
        public bool? Testmode { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}
