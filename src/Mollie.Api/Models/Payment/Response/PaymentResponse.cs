using System;
using System.Collections.Generic;
using System.Text.Json;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Payment.Response {
    public record PaymentResponse
    {
        /// <summary>
        /// Indicates the response contains a payment object. Will always contain payment for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The identifier uniquely referring to this payment. Mollie assigns this identifier randomly at payment creation
        /// time. For example tr_7UhSN1zuXS. Its ID will always be used by Mollie to refer to a certain payment.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The mode used to create this payment. Mode determines whether a payment is real or a test payment.
        /// </summary>
        public required Mode Mode { get; set; }

        /// <summary>
        /// The payment's date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// The payment's status. Please refer to the page about statuses for more info about which statuses occur at what
        /// point. See the Mollie.Api.Models.Payment.PaymentStatus class for a full list of known values.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// This object offers details about the status of a payment. Currently it is only available for point-of-sale payments.
        /// </summary>
        public StatusReason? StatusReason { get; set; }

        /// <summary>
        /// Whether or not the payment can be canceled.
        /// </summary>
        public bool? IsCancelable { get; set; }

        /// <summary>
        /// The date and time the payment became authorized, in ISO 8601 format. This parameter is omitted if the payment is not authorized (yet).
        /// </summary>
        public DateTime? AuthorizedAt { get; set; }

        /// <summary>
        /// The date and time the payment became paid, in ISO 8601 format. Null is returned if the payment isn't completed
        /// (yet).
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// The date and time the payment was cancelled, in ISO 8601 format. Null is returned if the payment isn't cancelled
        /// (yet).
        /// </summary>
        public DateTime? CanceledAt { get; set; }

        /// <summary>
        /// The date and time the payment was expired, in ISO 8601 format. Null is returned if the payment did not expire
        /// (yet).
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// The time until the payment will expire in ISO 8601 duration format.
        /// </summary>
        public DateTime? ExpiredAt { get; set; }

        /// <summary>
        /// The date and time the payment failed, in ISO 8601 format. This parameter is omitted if the payment did not fail (yet).
        /// </summary>
        public DateTime? FailedAt { get; set; }

        /// <summary>
        /// The amount of the payment, e.g. {"currency":"EUR", "value":"100.00"} for a €100.00 payment.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The total amount in EURO that is already refunded. For
        /// some payment methods, this
        /// amount may be higher than the payment amount, for example to allow reimbursement of the costs for a return shipment
        /// to the consumer.
        /// </summary>
        public Amount? AmountRefunded { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The remaining amount in EURO that can be refunded.
        /// </summary>
        public Amount? AmountRemaining { get; set; }

        /// <summary>
        /// The total amount that is already captured for this payment. Only available when this payment supports captures.
        /// </summary>
        public Amount? AmountCaptured { get; set; }

        /// <summary>
        /// The total amount that was charged back for this payment. Only available when the total charged back amount is not zero.
        /// </summary>
        public Amount? AmountChargedBack { get; set; }

        /// <summary>
        /// A short description of the payment. The description will be shown on the consumer's bank or card statement when
        /// possible.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The URL the consumer will be redirected to after completing or cancelling the payment process.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The optional redirect URL you provided during payment creation. Consumer that explicitly cancel the payment
        /// will be redirected to this URL if provided, or otherwise to the redirectUrl instead — see above.
        ///
        /// Mollie will always give you status updates via webhooks, including for the canceled status. This parameter
        /// is therefore entirely optional, but can be useful when implementing a dedicated consumer-facing flow to
        /// handle payment cancellations.
        ///
        /// The URL will be null for recurring payments.
        /// </summary>
        public string? CancelUrl { get; set; }

        /// <summary>
        /// The URL Mollie will call as soon an important status change takes place.
        /// </summary>
        public string? WebhookUrl { get; set; }

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
        /// An optional routing configuration that you provided, which enables you to route a successful payment, or part of the payment, to one or more connected accounts.
        /// Additionally, you can schedule (parts of) the payment to become available on the connected account on a future date.
        /// </summary>
        [JsonPropertyName("routing")]
        public IList<PaymentRoutingResponse>? Routings { get; set; }

        /// <summary>
        /// The payment method used for this payment, either forced on creation by specifying the method parameter, or chosen
        /// by the consumer our payment method selection screen. See the Mollie.Api.Models.Payment.PaymentMethod class for a
        /// full list of known values.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// The optional metadata you provided upon payment creation. Metadata can be used to link an order to a payment.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// The consumer's locale, either forced on creation by specifying the locale parameter, or detected by us during
        /// checkout.
        /// </summary>
        public string? Locale { get; set; }

        /// <summary>
        /// The customer’s ISO 3166-1 alpha-2 country code, detected by us during checkout. For example: BE.
        /// </summary>
        public string? CountryCode { get; set; }

        /// <summary>
        /// The identifier referring to the profile this payment was created on. For example, pfl_QkEhN94Ba.
        /// </summary>
        public required string ProfileId { get; set; }

        /// <summary>
        /// This optional field will contain the amount that will be settled to your account, converted to the currency your
        /// account is settled in. It follows the same syntax as the amount property.
        /// </summary>
        public Amount? SettlementAmount { get; set; }

        /// <summary>
        /// The identifier referring to the settlement this payment belongs to. For example, stl_BkEjN2eBb.
        /// </summary>
        public string? SettlementId { get; set; }

        /// <summary>
        /// The customerid of this payment
        /// </summary>
        public string? CustomerId { get; set; }

        /// <summary>
        /// Indicates which type of payment this is in a recurring sequence. Set to first for first payments that allow the customer to agree
        /// to automatic recurring charges taking place on their account in the future. Set to recurring for payments where the customer’s card
        /// is charged automatically. See the Mollie.Api.Models.Payment.SequenceType class for a full list of known values.
        /// </summary>
        public required string SequenceType { get; set; }

        /// <summary>
        /// Only available for recurring payments – If the payment is a recurring payment, this field will hold the ID of the
        /// mandate used to authorize the recurring payment.
        /// </summary>
        public string? MandateId { get; set; }

        /// <summary>
        /// Only available for recurring payments – When implementing the Subscriptions API, any recurring charges resulting
        /// from the subscription will hold the ID of the subscription that triggered the payment.
        /// </summary>
        public string? SubscriptionId { get; set; }

        /// <summary>
        /// If the payment was created for an order, the ID of that order will be part of the response.
        /// </summary>
        public string? OrderId { get; set; }

        /// <summary>
        /// The application fee, if the payment was created with one.
        /// </summary>
        public ApplicationFee? ApplicationFee { get; set; }

        /// <summary>
        /// Indicates whether the capture will be scheduled automatically or not. Set to manual for payments that can be captured
        /// manually using the Create capture endpoint. Set to automatic by default, which indicates the payment will be captured
        /// automatically, without having to separately request it.
        /// </summary>
        public string? CaptureMode { get; set; }

        /// <summary>
        /// Indicates the interval to wait before the payment is captured, for example 8 hours or 2 days. The capture delay will be
        /// added to the date and time the payment became authorized.
        /// </summary>
        public string? CaptureDelay { get; set; }

        /// <summary>
        /// Indicates the datetime on which the merchant has to have captured the payment, before we can no longer guarantee a
        /// successful capture, in ISO 8601 format. This parameter is omitted if the payment is not authorized (yet).
        /// </summary>
        public DateTime? CaptureBefore { get; set; }

        [JsonPropertyName("_embedded")]
        public PaymentEmbeddedResponse? Embedded { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the payment. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public PaymentResponseLinks Links { get; set; } = null!;

        public T? GetMetadata<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
            return Metadata != null ? JsonSerializer.Deserialize<T>(Metadata, jsonSerializerOptions) : default;
        }

        public override string ToString() {
            return $"Id: {Id} - Status: {Status} - Method: {Method} - Amount: {Amount}";
        }
    }
}
