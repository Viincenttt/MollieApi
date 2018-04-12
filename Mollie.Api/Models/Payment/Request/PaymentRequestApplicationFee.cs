namespace Mollie.Api.Models.Payment.Request
{
	public class PaymentRequestApplicationFee
	{
		/// <summary>
		///		The amount in EURO that the app wants to charge, e.g. 10.00 if the app would want to charge €10.00.
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		///		The description of the application fee. This will appear on settlement reports to the merchant and to you.
		/// </summary>
		public string Description { get; set; }
	}
}