using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mollie.Api.Models.PaymentLink.Request {
    public record PaymentLinkRequest
    {
        /// <summary>
        /// This description will also be used as the payment description and will be shown to your customer on their card or bank
        /// statement when possible.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The amount that you want to charge, e.g. {"currency":"EUR", "value":"1000.00"} if you would want to charge €1000.00.
        /// </summary>
        public Amount? Amount { get; set; }

        /// <summary>
        /// Optional - The URL your customer will be redirected to after completing the payment process.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send payment status updates to.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        ///	Oauth only - The website profile’s unique identifier, for example pfl_3RkSN1zuPE. This field is mandatory.
        /// </summary>
        public string? ProfileId { get; set; }

        /// <summary>
        /// The expiry date of the payment link in ISO 8601 format. For example: 2021-12-24T12:00:16+01:00.
        /// </summary>
        [JsonConverter(typeof(Iso8601DateTimeConverter))]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// An array of payment methods that are allowed to be used for this payment link. When this parameter is not
        /// provided or is an empty array, all enabled payment methods will be available.
        /// See the Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
        /// </summary>
        public IEnumerable<string>? AllowedMethods { get; set; }

        /// <summary>
        ///	Oauth only - Optional – 	Set this to true to only retrieve payment links made in test mode. By default, only live payment links are returned.
        /// </summary>
        public bool? Testmode { get; set; }

        public override string ToString() {
            return $"Amount: {Amount} - Description: {Description}";
        }
    }
}
