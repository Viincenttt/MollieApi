using System;
using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Payment;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Order {
    public class OrderResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains an order object. Will always contain order for this endpoint.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The order’s unique identifier, for example ord_vsKJpSsabw.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The profile the order was created on, for example pfl_v9hTwCvYqw.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// The payment method last used when paying for the order.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Payment.PaymentMethod? Method { get; set; }

        /// <summary>
        /// The mode used to create this order.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMode Mode { get; set; }

        /// <summary>
        /// The total amount of the order, including VAT and discounts.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// The amount captured, thus far. The captured amount will be settled to your account.
        /// </summary>
        public Amount AmountCaptured { get; set; }

        /// <summary>
        /// The total amount refunded, thus far.
        /// </summary>
        public Amount AmountRefunded { get; set; }

        /// <summary>
        /// The status of the order. 
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Whether or not the order can be (partially) canceled.
        /// </summary>
        public bool IsCancelable { get; set; }

        /// <summary>
        /// The person and the address the order is billed to. See below.
        /// </summary>
        public OrderAddressDetails BillingAddress { get; set; }

        /// <summary>
        /// The date of birth of your customer, if available.
        /// </summary>
        public DateTime? ConsumerDateOfBirth { get; set; }

        /// <summary>
        /// Your order number that was used when creating the order.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The person and the address the order is billed to. See below.
        /// </summary>
        public OrderAddressDetails ShippingAddress { get; set; }

        /// <summary>
        /// The locale used during checkout. 
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// The optional metadata you provided upon subscription creation. Metadata can for example be used to link a plan to a
        /// subscription.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to after completing or canceling the payment process.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// The URL Mollie will call as soon an important status change on the order takes place.
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// The order’s date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime CreatedAt { get; set; }

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

        public IEnumerable<OrderLineResponse> Lines { get; set; }

        [JsonProperty("_embedded")]
        public OrderEmbeddedResponse Embedded { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public OrderResponseLinks Links { get; set; }

        public T GetMetadata<T>(JsonSerializerSettings jsonSerializerSettings = null) {
            return JsonConvert.DeserializeObject<T>(this.Metadata, jsonSerializerSettings);
        }
    }
}