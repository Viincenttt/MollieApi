using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Session.Response {
    public record SessionResponse : IEntity {
        /// <summary>
        /// Indicates the response contains a session object.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The session's unique identifier, for example sub_rVKGtNd6s3.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The mode used to create this session. Mode determines whether the payments are real or test payments.
        /// </summary>
        public Mode Mode { get; set; }

        /// <summary>
        /// The client access token for the session.
        /// </summary>
        public required string ClientAccessToken { get; set; }

        /// <summary>
        /// The session's current status, depends on whether the customer has a pending, valid or invalid mandate.
        /// See the Mollie.Api.Models.Session.SessionStatus class for a full list of known values.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// The constant amount that is charged with each session payment.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The description of the session.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to after the payment process.
        /// It could make sense for the redirectUrl to contain a unique identifier – like your order ID – so you can show the right page referencing the order when your customer returns.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to when the customer explicitly cancels the payment.
        /// If this URL is not provided, the customer will be redirected to the redirectUrl instead — see above.
        /// </summary>
        public string? CancelUrl { get; set; }

        public SessionPaymentResponse? Payment { get; set; }

        /// <summary>
        /// Optionally provide the order lines for the payment. Each line contains details such as a description of the item ordered and its price.
        /// All lines must have the same currency as the payment.
        /// </summary>
        public List<PaymentLine>? Lines { get; set; }

        /// <summary>
        /// The customer's billing address details. We advise to provide these details to improve fraud protection and conversion. This is
        /// particularly relevant for card payments.
        /// </summary>
        public PaymentAddressDetails? BillingAddress { get; set; }

        /// <summary>
        /// The customer's shipping address details. We advise to provide these details to improve fraud protection and conversion. This is
        /// particularly relevant for card payments.
        /// </summary>
        public PaymentAddressDetails? ShippingAddress { get; set; }

        /// <summary>
        /// The customer this session belongs to.
        /// </summary>
        public required string CustomerId { get; set; }

        /// <summary>
        /// Indicates which type of payment this is in a recurring sequence. Set to first for first payments that allow the customer to agree
        /// to automatic recurring charges taking place on their account in the future. Set to recurring for payments where the customer’s card
        /// is charged automatically. See the Mollie.Api.Models.Payment.SequenceType class for a full list of known values.
        /// </summary>
        public required string SequenceType { get; set; }

        /// <summary>
        /// The optional metadata you provided upon session creation. Metadata can for example be used to link a plan to a
        /// session.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the session. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required SessionResponseLinks Links { get; set; }

        public T? GetMetadata<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
            return Metadata != null ? JsonSerializer.Deserialize<T>(Metadata, jsonSerializerOptions) : default;
        }
    }
}
