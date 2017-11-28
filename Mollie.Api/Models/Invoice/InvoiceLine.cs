using System;

namespace Mollie.Api.Models.Invoice
{
	public class InvoiceLine
	{
		/// <summary>
		/// The administrative period (YYYY) on which the line should be booked.
		/// </summary>
		public DateTime Period { get; set; }

		/// <summary>
		/// Description of the product.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Number of products invoiced (usually number of payments).
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Optional – VAT percentage rate that applies to this product.
		/// </summary>
		public decimal VatPercentage { get; set; }

		/// <summary>
		/// Amount excluding VAT.
		/// </summary>
		public decimal Amount { get; set; }
	}
}