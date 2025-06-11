using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Mollie.Api.Models.Payment;

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
        /// The minimum amount of the payment link. This property is only allowed when there is no amount provided.
        /// The customer will be prompted to enter a value greater than or equal to the minimum amount.
        /// </summary>
        public Amount? MinimumAmount { get; set; }

        /// <summary>
        /// Optional - The URL your customer will be redirected to after completing the payment process.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send payment status updates to.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Optionally provide the order lines for the payment. Each line contains details such as a description of the item ordered and its price.
        /// All lines must have the same currency as the payment.
        /// </summary>
        public List<PaymentLine>? Lines { get; set; }

        /// <summary>
        /// The customer's billing address details. We advise to provide these details to improve fraud protection and conversion.
        /// </summary>
        public PaymentAddressDetails? BillingAddress { get; set; }

        /// <summary>
        /// The customer's shipping address details. We advise to provide these details to improve fraud protection and conversion.
        /// </summary>
        public PaymentAddressDetails? ShippingAddress { get; set; }

        /// <summary>
        ///	Oauth only - The website profile’s unique identifier, for example pfl_3RkSN1zuPE. This field is mandatory.
        /// </summary>
        public string? ProfileId { get; set; }

        /// <summary>
        /// Indicates whether the payment link is reusable. If this field is set to true, customers can make multiple
        /// payments using the same link. If no value is specified, the field defaults to false, allowing only a single
        /// payment per link.
        /// </summary>
        public bool? Reusable { get; set; }

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
        /// With Mollie Connect you can charge fees on payment links that your app is processing on behalf of other
        /// Mollie merchants.
        ///
        /// If you use OAuth to create payment links on a connected merchant's account, you can charge
        /// a fee using this applicationFee parameter. If a payment on the payment link succeeds, the fee will be
        /// deducted from the merchant's balance and sent to your own account balance.
        /// </summary>
        public ApplicationFee? ApplicationFee { get; set; }

        /// <summary>
        ///	Oauth only - Optional – 	Set this to true to only retrieve payment links made in test mode. By default, only live payment links are returned.
        /// </summary>
        public bool? Testmode { get; set; }

        public override string ToString() {
            return $"Amount: {Amount} - Description: {Description}";
        }
    }
}
