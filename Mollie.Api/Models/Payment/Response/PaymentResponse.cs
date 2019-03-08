﻿using System;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Payment.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment.Response {
    public class PaymentResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a payment object. Will always contain payment for this endpoint.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The identifier uniquely referring to this payment. Mollie assigns this identifier randomly at payment creation
        /// time. For example tr_7UhSN1zuXS. Its ID will always be used by Mollie to refer to a certain payment.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The mode used to create this payment. Mode determines whether a payment is real or a test payment.
        /// </summary>
        public PaymentMode Mode { get; set; }

        /// <summary>
        /// The payment's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The payment's status. Please refer to the page about statuses for more info about which statuses occur at what
        /// point.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentStatus? Status { get; set; }

        /// <summary>
        /// Whether or not the payment can be canceled.
        /// </summary>
        public bool IsCancelable { get; set; }

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
        public Amount Amount { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The total amount in EURO that is already refunded. For
        /// some payment methods, this
        /// amount may be higher than the payment amount, for example to allow reimbursement of the costs for a return shipment
        /// to the consumer.
        /// </summary>
        public Amount AmountRefunded { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The remaining amount in EURO that can be refunded.
        /// </summary>
        public Amount AmountRemaining { get; set; }

        /// <summary>
        /// The total amount that is already captured for this payment. Only available when this payment supports captures.
        /// </summary>
        public Amount AmountCaptured { get; set; }

        /// <summary>
        /// A short description of the payment. The description will be shown on the consumer's bank or card statement when
        /// possible.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The URL the consumer will be redirected to after completing or cancelling the payment process.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// The URL Mollie will call as soon an important status change takes place.
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        ///     The payment method used for this payment, either forced on creation by specifying the method parameter, or chosen
        ///     by the consumer our
        ///     payment method selection screen.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMethod? Method { get; set; }

        /// <summary>
        /// The optional metadata you provided upon payment creation. Metadata can be used to link an order to a payment.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        /// <summary>
        /// The consumer's locale, either forced on creation by specifying the locale parameter, or detected by us during
        /// checkout.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// The customer’s ISO 3166-1 alpha-2 country code, detected by us during checkout. For example: BE.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The identifier referring to the profile this payment was created on. For example, pfl_QkEhN94Ba.
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// This optional field will contain the amount that will be settled to your account, converted to the currency your
        /// account is settled in. It follows the same syntax as the amount property.
        /// </summary>
        public Amount SettlementAmount { get; set; }

        /// <summary>
        /// The identifier referring to the settlement this payment belongs to. For example, stl_BkEjN2eBb.
        /// </summary>
        public string SettlementId { get; set; }

        /// <summary>
        /// The customerid of this payment
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Indicates which type of payment this is in a recurring sequence. Set to first for first payments that allow the customer to agree 
        /// to automatic recurring charges taking place on their account in the future. Set to recurring for payments where the customer’s card
        /// is charged automatically.
        /// </summary>
        public SequenceType? SequenceType { get; set; }


        /// <summary>
        /// Only available for recurring payments – If the payment is a recurring payment, this field will hold the ID of the
        /// mandate used to authorize the recurring payment.
        /// </summary>
        public string MandateId { get; set; }

        /// <summary>
        /// Only available for recurring payments – When implementing the Subscriptions API, any recurring charges resulting
        /// from the subscription will hold the ID of the subscription that triggered the payment.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// If the payment was created for an order, the ID of that order will be part of the response.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// The application fee, if the payment was created with one.
        /// </summary>
        public PaymentRequestApplicationFee ApplicationFee { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the payment. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public PaymentResponseLinks Links { get; set; }
        
        public T GetMetadata<T>(JsonSerializerSettings jsonSerializerSettings = null) {
            return JsonConvert.DeserializeObject<T>(this.Metadata, jsonSerializerSettings);
        }

        public override string ToString() {
            return $"Id: {this.Id} - Status: {this.Status} - Method: {this.Method} - Amount: {this.Amount}";
        }
    }
}