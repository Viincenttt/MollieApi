using System;
using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Payment;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Customer {
    public class CustomerResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a customer object. Will always contain customer for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        /// The customer's unique identifier, for example cst_4pmbK7CqtT.
        /// Store this identifier for later recurring payments.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// The mode used to create this payment. Mode determines whether a payment is real or a test payment.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Mode Mode { get; set; }

        /// <summary>
        /// Name of your customer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// E-mailaddress of your customer.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Allows you to preset the language to be used in the payment screens shown to the consumer. If this parameter was not 
        /// provided when the customer was created, the browser language will be used instead in the payment flow (which is usually
        /// more accurate).
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Optional metadata. Use this if you want Mollie to store additional info.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        /// <summary>
        /// Payment methods that the customer recently used for payments.
        /// </summary>
        public List<string> RecentlyUsedMethods { get; set; }

        /// <summary>
        /// DateTime when user was created.
        /// </summary>
        public required DateTime CreatedAt { get; init; }

        /// <summary>
        /// An object with several URL objects relevant to the customer. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public CustomerResponseLinks Links { get; set; }

        public T? GetMetadata<T>(JsonSerializerSettings? jsonSerializerSettings = null) {
            return Metadata != null ? JsonConvert.DeserializeObject<T>(Metadata, jsonSerializerSettings) : default;
        }
    }
}