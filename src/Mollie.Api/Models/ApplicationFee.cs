namespace Mollie.Api.Models {
	public class ApplicationFee {
		/// <summary>
		///	The amount in EURO that the app wants to charge, e.g. 10.00 if the app would want to charge €10.00.
		/// </summary>
		public required Amount Amount { get; init; }

		/// <summary>
		///	The description of the application fee. This will appear on settlement reports to the merchant and to you.
		/// </summary>
		public required string Description { get; init; }
	}
}