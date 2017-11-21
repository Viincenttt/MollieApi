namespace Mollie.Api.Models.Settlement
{
	public class SettlementPeriodRevenue
	{
		/// <summary>
		/// A description of the subtotal.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The paid costs for this payment method, further divided in net (excludes VAT), vat, and gross (includes VAT).
		/// </summary>
		public SettlementAmount Amount { get; set; }

		/// <summary>
		/// The number of times costs were made for this payment method.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// The payment method ID, if applicable.
		/// </summary>
		public Payment.PaymentMethod Method { get; set; }
	}
}