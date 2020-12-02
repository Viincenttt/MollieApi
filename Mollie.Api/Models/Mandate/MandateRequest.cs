﻿using System;

namespace Mollie.Api.Models.Mandate {
    public class MandateRequest {
        /// <summary>
        /// Payment method of the mandate - Possible values: `directdebit` `paypal`
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Required - Name of consumer you add to the mandate
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Optional - The date when the mandate was signed.
        /// </summary>
        public DateTime? SignatureDate { get; set; }

        /// <summary>
        /// Optional - A custom reference
        /// </summary>
        public string MandateReference { get; set; }
    }
}