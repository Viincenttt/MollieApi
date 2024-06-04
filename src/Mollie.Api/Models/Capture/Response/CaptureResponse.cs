using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Capture.Response
{
    public record CaptureResponse {
        /// <summary>
        /// Indicates the response contains a capture object. Will always contain capture for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The capture’s unique identifier, for example cpt_4qqhO89gsT.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The mode used to create this capture.
        /// Possible values: live test
        /// </summary>
        public required string Mode { get; set; }

        /// <summary>
        /// The amount captured.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The capture’s status.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// This optional field will contain the amount that will be settled to your account, converted to the currency your account is settled in. It follows the same syntax as the amount property.
        /// </summary>
        public required Amount SettlementAmount { get; set; }

        /// <summary>
        /// The unique identifier of the payment this capture was created for, for example: tr_7UhSN1zuXS
        /// </summary>
        public required string PaymentId { get; set; }

        /// <summary>
        /// The unique identifier of the shipment that triggered the creation of this capture, for example: shp_3wmsgCJN4U
        /// </summary>
        public required string ShipmentId { get; set; }

        /// <summary>
        /// The unique identifier of the settlement this capture was settled with, for example: stl_jDk30akdN
        /// </summary>
        public required string SettlementId { get; set; }

        /// <summary>
        /// The capture’s date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// The optional metadata you provided upon payment creation. Metadata can be used to link an order to a payment.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// The optional metadata you provided upon capture creation. Metadata can for example be used to link an bookkeeping ID to a capture.
        /// </summary>
        [JsonProperty("_links")]
        public required CaptureResponseLinks Links { get; set; }
    }
}
