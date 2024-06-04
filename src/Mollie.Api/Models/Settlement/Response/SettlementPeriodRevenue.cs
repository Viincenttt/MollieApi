namespace Mollie.Api.Models.Settlement.Response
{
	public record SettlementPeriodRevenue
	{
		/// <summary>
		/// A description of the subtotal.
		/// </summary>
		public required string Description { get; set; }

        /// <summary>
        /// The net total of received funds for this payment method (excludes VAT).
        /// </summary>
        public required Amount AmountNet { get; set; }

        /// <summary>
        /// The VAT amount applicable to the revenue.
        /// </summary>
        public required Amount AmountVat { get; set; }

        /// <summary>
        /// The gross total of received funds for this payment method (includes VAT).
        /// </summary>
        public required Amount AmountGross { get; set; }

		/// <summary>
		/// The number of times costs were made for this payment method.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// The payment method ID, if applicable - See the Mollie.Api.Models.Payment.PaymentMethod
		/// class for a full list of known values.
		/// </summary>
		public string? Method { get; set; }
	}
}
