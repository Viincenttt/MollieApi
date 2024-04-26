namespace Mollie.Api.Models.Invoice {
	public record InvoiceLine {
		/// <summary>
		/// The administrative period (YYYY) on which the line should be booked.
		/// </summary>
		public required string Period { get; init; }

		/// <summary>
		/// Description of the product.
		/// </summary>
		public required string Description { get; init; }

		/// <summary>
		/// Number of products invoiced (usually number of payments).
		/// </summary>
		public required int Count { get; init; }

		/// <summary>
		/// Optional – VAT percentage rate that applies to this product.
		/// </summary>
		public required decimal VatPercentage { get; init; }

		/// <summary>
		/// Amount excluding VAT.
		/// </summary>
		public required Amount Amount { get; init; }
	}
}