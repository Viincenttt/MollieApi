using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Capture
{
    public class CaptureResponse : IResponseObject{
        /// <summary>
        /// Indicates the response contains a capture object. Will always contain capture for this endpoint.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The capture’s unique identifier, for example cpt_4qqhO89gsT.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The mode used to create this capture.
        /// Possible values: live test
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// The amount captured.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// This optional field will contain the amount that will be settled to your account, converted to the currency your account is settled in. It follows the same syntax as the amount property.
        /// </summary>
        public Amount SettlementAmount { get; set; }

        /// <summary>
        /// The unique identifier of the payment this capture was created for, for example: tr_7UhSN1zuXS
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// The unique identifier of the shipment that triggered the creation of this capture, for example: shp_3wmsgCJN4U
        /// </summary>
        public string ShipmentId { get; set; }

        /// <summary>
        /// The unique identifier of the settlement this capture was settled with, for example: stl_jDk30akdN
        /// </summary>
        public string SettlementId { get; set; }

        /// <summary>
        /// The capture’s date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The optional metadata you provided upon subscription creation. Metadata can for example be used to link a plan to a
        /// subscription.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public CaptureResponseLinks Links { get; set; }

        public T GetMetadata<T>(JsonSerializerSettings jsonSerializerSettings = null) {
            return JsonConvert.DeserializeObject<T>(this.Metadata, jsonSerializerSettings);
        }
    }
}