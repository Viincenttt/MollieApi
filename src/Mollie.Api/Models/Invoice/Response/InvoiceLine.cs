namespace Mollie.Api.Models.Invoice.Response {
	public record InvoiceLine {
		/// <summary>
		/// The administrative period (YYYY) on which the line should be booked.
		/// </summary>
		public required string Period { get; set; }

		/// <summary>
		/// Description of the product.
		/// </summary>
		public required string Description { get; set; }

		/// <summary>
		/// Number of products invoiced (usually number of payments).
		/// </summary>
		public required int Count { get; set; }

		/// <summary>
		/// Optional – VAT percentage rate that applies to this product.
		/// </summary>
		public required decimal VatPercentage { get; set; }

		/// <summary>
		/// Amount excluding VAT.
		/// </summary>
		public required Amount Amount { get; set; }
	}
}
