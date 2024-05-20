using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using System;

namespace Mollie.Api.Models.PaymentLink.Request {
    public record PaymentLinkRequest
    {
        /// <summary>
        /// The amount that you want to charge, e.g. {"currency":"EUR", "value":"1000.00"} if you would want to charge €1000.00.
        /// </summary>
        public Amount? Amount { get; init; }

        /// <summary>
        /// This description will also be used as the payment description and will be shown to your customer on their card or bank
        /// statement when possible.
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// Optional - The URL your customer will be redirected to after completing the payment process.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send payment status updates to.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// The expiry date of the payment link in ISO 8601 format. For example: 2021-12-24T12:00:16+01:00.
        /// </summary>
        [JsonConverter(typeof(Iso8601DateTimeConverter))]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        ///	Oauth only - The website profile’s unique identifier, for example pfl_3RkSN1zuPE. This field is mandatory.
        /// </summary>
        public string? ProfileId { get; set; }

        /// <summary>
        ///	Oauth only - Optional – 	Set this to true to only retrieve payment links made in test mode. By default, only live payment links are returned.
        /// </summary>
        public bool? Testmode { get; set; }

        public override string ToString() {
            return $"Amount: {Amount} - Description: {Description}";
        }
    }
}
