﻿using System;

namespace Mollie.Api.Models.Mandate.Request {
    public record MandateRequest {
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
        public DateTime? SignatureDate { get; set; }

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
