namespace Mollie.Api.Models.Settlement
{
	public class SettlementAmount
	{
		public decimal Net { get; set; }

		public decimal Vat { get; set; }

		public decimal Gross { get; set; }
	}
}