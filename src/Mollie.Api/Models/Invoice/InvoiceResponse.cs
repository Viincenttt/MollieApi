﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Invoice {
	public record InvoiceResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains an invoice object. Will always contain invoice for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

		/// <summary>
		/// The invoice's unique identifier, for example inv_FrvewDA3Pr.
		/// </summary>
		public required string Id { get; init; }

		/// <summary>
		/// The reference number of the invoice. An example value would be: 2016.10000.
		/// </summary>
		public required string Reference { get; init; }

		/// <summary>
		/// Optional – The VAT number to which the invoice was issued to (if applicable).
		/// </summary>
		public string? VatNumber { get; set; }

		/// <summary>
		/// Status of the invoices - See the Mollie.Api.Models.Invoice.InvoiceStatus class for
		/// a full list of known values
		/// </summary>
		public required string Status { get; init; }

		/// <summary>
		/// The invoice date (in YYYY-MM-DD format).
		/// </summary>
		public required string IssuedAt { get; init; }

		/// <summary>
		/// Optional – The date on which the invoice was paid (in YYYY-MM-DD format). Only for paid invoices.
		/// </summary>
		public string? PaidAt { get; set; }

		/// <summary>
		/// Optional – The date on which the invoice is due (in YYYY-MM-DD format). Only for due invoices.
		/// </summary>
		public string? DueAt { get; set; }

        /// <summary>
        /// Total amount of the invoice excluding VAT, e.g. {"currency":Currency.EUR, "value":"100.00"}.
        /// </summary>
        public required Amount NetAmount { get; init; }

        /// <summary>
        /// VAT amount of the invoice. Only for merchants registered in the Netherlands. For EU merchants, VAT 
        /// will be shifted to recipient; article 44 and 196 EU VAT Directive 2006/112. For merchants outside the 
        /// EU, no VAT will be charged.
        /// </summary>
        public required Amount VatAmount { get; init; }

        /// <summary>
        /// Total amount of the invoice including VAT.
        /// </summary>
        public required Amount GrossAmount { get; init; }

        /// <summary>
        /// The collection of products which make up the invoice.
        /// </summary>
        public required List<InvoiceLine> Lines { get; init; }

		/// <summary>
		/// Useful URLs to related resources.
		/// </summary>
		[JsonProperty("_links")]
		public required InvoiceResponseLinks Links { get; init; }
	}
}