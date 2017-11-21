namespace Mollie.Api.Models.Settlement
{
	public class SettlementPeriodCosts
	{
		/// <summary>
		/// A description of the subtotal.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The received subtotal for this payment method, further divided in net (excludes VAT), vat, and gross (includes VAT).
		/// </summary>
		public SettlementAmount Amount { get; set; }

		/// <summary>
		/// The number of payments received for this payment method.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// The service rates, further divided into fixed and variable costs.
		/// </summary>
		public SettlementPeriodCostsRate Rate { get; set; }

		/// <summary>
		/// The payment method ID, if applicable.
		/// </summary>
		public Payment.PaymentMethod Method { get; set; }
	}
}