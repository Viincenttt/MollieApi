namespace Mollie.Api.Models.Invoice {
	public class InvoiceResponseLinks {
        /// <summary>
        /// The API resource URL of the invoice itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The URL to the PDF version of the invoice. The URL will expire after 60 minutes.
        /// </summary>
		public UrlObject Pdf { get; set; }

        /// <summary>
        /// The URL to the invoice retrieval endpoint documentation.
        /// </summary>
		public UrlObject Documentation { get; set; }
	}
}