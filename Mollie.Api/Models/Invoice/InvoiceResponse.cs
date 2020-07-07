using System.Collections.Generic;
using Mollie.Api.Models.Settlement;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Invoice {
	public class InvoiceResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains an invoice object. Will always contain invoice for this endpoint.
        /// </summary>
        public string Resource { get; set; }

		/// <summary>
		/// The invoice's unique identifier, for example inv_FrvewDA3Pr.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The reference number of the invoice. An example value would be: 2016.10000.
		/// </summary>
		public string Reference { get; set; }

		/// <summary>
		/// Optional – The VAT number to which the invoice was issued to (if applicable).
		/// </summary>
		public string VatNumber { get; set; }

		/// <summary>
		/// Status of the invoices
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// The invoice date (in YYYY-MM-DD format).
		/// </summary>
		public string IssuedAt { get; set; }

		/// <summary>
		/// Optional – The date on which the invoice was paid (in YYYY-MM-DD format). Only for paid invoices.
		/// </summary>
		public string PaidAt { get; set; }

		/// <summary>
		/// Optional – The date on which the invoice is due (in YYYY-MM-DD format). Only for due invoices.
		/// </summary>
		public string DueAt { get; set; }

        /// <summary>
        /// Total amount of the invoice excluding VAT, e.g. {"currency":Currency.EUR, "value":"100.00"}.
        /// </summary>
        public Amount NetAmount { get; set; }

        /// <summary>
        /// VAT amount of the invoice. Only for merchants registered in the Netherlands. For EU merchants, VAT 
        /// will be shifted to recipient; article 44 and 196 EU VAT Directive 2006/112. For merchants outside the 
        /// EU, no VAT will be charged.
        /// </summary>
        public Amount VatAmount { get; set; }

        /// <summary>
        /// Total amount of the invoice including VAT.
        /// </summary>
        public Amount GrossAmount { get; set; }

        /// <summary>
        /// Only available if you require this field to be included – The collection of products which make up the invoice.
        /// </summary>
        public List<InvoiceLine> Lines { get; set; }

		/// <summary>
		/// Only available if you require this field to be included – A array of settlements that were invoiced on this invoice.
		/// You need the settlements.read permission for this field.
		/// </summary>
		public List<SettlementResponse> Settlements { get; set; }

		/// <summary>
		/// Useful URLs to related resources.
		/// </summary>
		[JsonProperty("_links")]
		public InvoiceResponseLinks Links { get; set; }
	}
}