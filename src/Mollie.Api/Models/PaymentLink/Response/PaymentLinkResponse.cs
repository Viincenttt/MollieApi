using System;
using Newtonsoft.Json;

namespace Mollie.Api.Models.PaymentLink.Response
{
    public record PaymentLinkResponse : IResponseObject
    {
        /// <summary>
        /// Indicates the response contains a payment object. Will always contain payment-link for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        /// The identifier uniquely referring to this payment link. Mollie assigns this identifier at creation time. 
        /// For example pl_4Y0eZitmBnQ6IDoMqZQKh. Its ID will always be used by Mollie to refer to a certain payment link.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        ///A short description of the payment link. The description is visible in the Dashboard and will be shown on the 
        ///customer’s bank or card statement when possible. This description will eventual been used as payment description.
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// The mode used to create this payment link. Mode determines whether a payment link is real (live mode) or a test payment link.
        /// </summary>
        public Mode Mode { get; set; }

        /// <summary>
        /// The identifier referring to the profile this payment link was created on. For example, pfl_QkEhN94Ba.
        /// </summary>
        public string? ProfileId { get; set; }

        /// <summary>
        /// The amount of the payment link, e.g. {"currency":"EUR", "value":"100.00"} for a €100.00 payment link.
        /// </summary>
        public required Amount Amount { get; init; }

        /// <summary>
        /// The URL your customer will be redirected to after completing the payment process.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The URL Mollie will call as soon an important status change takes place.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// The payment link’s date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The date and time the payment link became paid, in ISO 8601 format.
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// The date and time the payment link last status change, in ISO 8601 format.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The expiry date and time of the payment link, in ISO 8601 format.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the payment. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required PaymentLinkResponseLinks Links { get; init; }
         
        public override string ToString()
        {
            return $"Id: {Id} - Description: {Description} - Amount: {Amount}";
        }
    }
}
