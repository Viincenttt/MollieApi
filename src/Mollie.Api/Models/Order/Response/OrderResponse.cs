using System;
using System.Collections.Generic;
using System.Text.Json;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Order.Response {
    public record OrderResponse {
        /// <summary>
        /// Indicates the response contains an order object. Will always contain order for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The order’s unique identifier, for example ord_vsKJpSsabw.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The profile the order was created on, for example pfl_v9hTwCvYqw.
        /// </summary>
        public required string ProfileId { get; set; }

        /// <summary>
        /// The payment method last used when paying for the order - See the
        /// Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// The mode used to create this order.
        /// </summary>
        public Mode Mode { get; set; }

        /// <summary>
        /// The total amount of the order, including VAT and discounts.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The amount captured, thus far. The captured amount will be settled to your account.
        /// </summary>
        public Amount? AmountCaptured { get; set; }

        /// <summary>
        /// The total amount refunded, thus far.
        /// </summary>
        public Amount? AmountRefunded { get; set; }

        /// <summary>
        /// The status of the order - See the Mollie.Api.Models.Order.OrderStatus class for a full list of known values.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// Whether or not the order can be (partially) canceled.
        /// </summary>
        public required bool IsCancelable { get; set; }

        /// <summary>
        /// The person and the address the order is billed to. See below.
        /// </summary>
        public OrderAddressDetails? BillingAddress { get; set; }

        /// <summary>
        /// The date of birth of your customer, if available.
        /// </summary>
        public DateTime? ConsumerDateOfBirth { get; set; }

        /// <summary>
        /// Your order number that was used when creating the order.
        /// </summary>
        public required string OrderNumber { get; set; }

        /// <summary>
        /// The person and the address the order is billed to. See below.
        /// </summary>
        public OrderAddressDetails? ShippingAddress { get; set; }

        /// <summary>
        /// The locale used during checkout.
        /// </summary>
        public required string Locale { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data
        /// alongside the order. Whenever you fetch the order with our API, we’ll also include the
        /// metadata. You can use up to approximately 1kB.
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to after completing or canceling the payment process.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The optional redirect URL you provided during payment creation. Consumer that explicitly cancel the
        /// order will be redirected to this URL if provided, or otherwise to the redirectUrl instead — see above.
        ///
        /// Mollie will always give you status updates via webhooks, including for the canceled status. This parameter
        /// is therefore entirely optional, but can be useful when implementing a dedicated consumer-facing flow to
        /// handle order cancellations.
        ///
        /// The URL will be null for recurring orders.
        /// </summary>
        public string? CancelUrl { get; set; }

        /// <summary>
        /// The URL Mollie will call as soon an important status change on the order takes place.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// The order’s date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time the order will expire, in ISO 8601 format. Note that you have until this date to fully ship the
        /// order.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// If the order is expired, the time of expiration will be present in ISO 8601 format.
        /// </summary>
        public DateTime? ExpiredAt { get; set; }

        /// <summary>
        /// If the order has been paid, the time of payment will be present in ISO 8601 format.
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// If the order has been authorized, the time of authorization will be present in ISO 8601 format.
        /// </summary>
        public DateTime? AuthorizedAt { get; set; }

        /// <summary>
        /// If the order has been canceled, the time of cancellation will be present in ISO 8601 format.
        /// </summary>
        public DateTime? CanceledAt { get; set; }

        /// <summary>
        /// If the order is completed, the time of completion will be present in ISO 8601 format.
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        public required IEnumerable<OrderLineResponse> Lines { get; set; }

        [JsonPropertyName("_embedded")]
        public OrderEmbeddedResponse? Embedded { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required OrderResponseLinks Links { get; set; }

        public T? GetMetadata<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
            return Metadata != null ? JsonSerializer.Deserialize<T>(Metadata, jsonSerializerOptions) : default;
        }
    }
}
