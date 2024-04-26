﻿using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Invoice {
	public class InvoiceResponseLinks {
        /// <summary>
        /// The API resource URL of the invoice itself.
        /// </summary>
        public required UrlObjectLink<InvoiceResponse> Self { get; init; }

        /// <summary>
        /// The URL to the PDF version of the invoice. The URL will expire after 60 minutes.
        /// </summary>
		public UrlLink? Pdf { get; set; }

        /// <summary>
        /// The URL to the invoice retrieval endpoint documentation.
        /// </summary>
		public required UrlLink Documentation { get; init; }
	}
}