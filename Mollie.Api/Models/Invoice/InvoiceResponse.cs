using System;
using System.Collections.Generic;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Settlement;

namespace Mollie.Api.Models.Invoice
{
	public class InvoiceResponse
	{
		/// <summary>
		/// Indicates the response contains a invoice.
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
		public InvoiceStatus Status { get; set; }

		/// <summary>
		/// The invoice date (in YYYY-MM-DD format).
		/// </summary>
		public DateTime IssueDate { get; set; }

		/// <summary>
		/// Optional – The date on which the invoice was paid (in YYYY-MM-DD format). Only for paid invoices.
		/// </summary>
		public DateTime PaidDate { get; set; }

		/// <summary>
		/// Optional – The date on which the invoice is due (in YYYY-MM-DD format). Only for due invoices.
		/// </summary>
		public DateTime DueDate { get; set; }

		/// <summary>
		/// The total amount of the invoice with and without VAT.
		/// </summary>
		public InvoiceAmount Amount { get; set; }

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
		public InvoiceLinks Links { get; set; }
	}
}