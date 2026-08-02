using System;
using System.Text.Json.Serialization;
using Mollie.Api.Framework;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Mandate.Request {
    public record MandateRequest : ITestModeRequest {
        /// <summary>
        /// Payment method of the mandate - Possible values: `directdebit` `paypal`
        /// </summary>
        public required string Method { get; set; }

        /// <summary>
        /// Required - Name of consumer you add to the mandate
        /// </summary>
        public required string ConsumerName { get; set; }

        /// <summary>
        /// Optional - The date when the mandate was signed.
        /// </summary>
#if NET8_0_OR_GREATER
        public DateOnly? SignatureDate { get; set; }
#else
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime? SignatureDate { get; set; }
#endif

        /// <summary>
        /// Optional - A custom reference
        /// </summary>
        public string? MandateReference { get; set; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this mandate a test mandate.
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
