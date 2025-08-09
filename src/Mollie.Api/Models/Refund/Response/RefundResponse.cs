using System;
using System.Collections.Generic;
using System.Text.Json;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Refund.Response {
    public record RefundResponse : IEntity {
        /// <summary>
        /// Indicates the response contains a refund object. Will always contain refund for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The refund's unique identifier, for example re_4qqhO89gsT.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The description of the refund that may be shown to the consumer, depending on the payment method used.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The amount refunded to the consumer with this refund.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// This optional field will contain the amount that will be deducted from your account balance, converted
        /// to the currency your account is settled in. It follows the same syntax as the amount property. Note that
        /// for refunds, the value key of settlementAmount will be negative. Any amounts not settled by Mollie will
        /// not be reflected in this amount, e.g. PayPal refunds.
        /// </summary>
        public Amount? SettlementAmount { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the refund. Whenever
        /// you fetch the refund with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// Since refunds may be delayed for certain payment methods, the refund carries a status field. See the
        /// Mollie.Api.Models.Refund.RefundStatus class for a full list of known values.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// With Mollie Connect you can charge fees on payments that your app is processing on behalf of other Mollie merchants,
        /// by providing the routing object during payment creation. When creating refunds for these routed payments, by default
        /// the full amount is deducted from your balance.If you want to pull back the funds that were routed to the connected
        /// merchant(s), you can set this parameter to true when issuing a full refund. For more fine-grained control and for
        /// partial refunds, use the routingReversals parameter instead.
        /// </summary>
        public bool? ReverseRouting { get; set; }

        /// <summary>
        /// When creating refunds for routed payments, by default the full amount is deducted from your balance. If you want to
        /// pull back funds from the connected merchant(s), you can use this parameter to specify what amount needs to be
        /// reversed from which merchant(s). If you simply want to fully reverse the routed funds, you can also use the
        /// reverseRouting parameter instead.
        /// </summary>
        public IList<RoutingReversal>? RoutingReversals { get; set; }

        /// <summary>
        /// The unique identifier of the payment this refund was created for. For example: tr_7UhSN1zuXS. The full
        /// payment object can be retrieved via the payment URL in the _links object.
        /// </summary>
        public required string PaymentId { get; set; }

        /// <summary>
        /// The identifier referring to the settlement this payment was settled with. For example, stl_BkEjN2eBb.
        /// This field is omitted if the refund is not settled (yet).
        /// </summary>
        public string? SettlementId { get; set; }

        /// <summary>
        /// The date and time the refund was issued, in ISO 8601 format.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the refund. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required RefundResponseLinks Links { get; set; }

        public T? GetMetadata<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
            return Metadata != null ? JsonSerializer.Deserialize<T>(Metadata, jsonSerializerOptions) : default;
        }
    }
}
