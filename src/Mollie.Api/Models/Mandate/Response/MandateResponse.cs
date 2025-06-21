using System;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Mandate.Response {
    public record MandateResponse {
        /// <summary>
        /// Indicates the response contains a mandate object. Will always contain mandate for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// Unique identifier of you mandate.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Current status of mandate - See the Mollie.Api.Models.Mandate.MandateStatus class for a full
        /// list of known values.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// Payment method of the mandate - See the Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
        /// </summary>
        public required string Method { get; set; }

        /// <summary>
        /// The mandate’s custom reference, if this was provided when creating the mandate.
        /// </summary>
        public string? MandateReference { get; set; }

        /// <summary>
        /// The signature date of the mandate in YYYY-MM-DD format.
        /// </summary>
        public string? SignatureDate { get; set; }

        /// <summary>
        /// DateTime when mandate was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the mandate. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required MandateResponseLinks Links { get; set; }
    }
}
