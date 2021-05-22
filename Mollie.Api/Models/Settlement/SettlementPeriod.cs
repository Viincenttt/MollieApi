using System.Collections.Generic;

namespace Mollie.Api.Models.Settlement {
	public class SettlementPeriod {
		/// <summary>
		/// The total revenue for each payment method during this period.
		/// </summary>
		public List<SettlementPeriodRevenue> Revenue { get; set; }

		/// <summary>
		/// The fees withheld for each payment method during this period.
		/// </summary>
		public List<SettlementPeriodCosts> Costs { get; set; }
		
		/// <summary>
		/// The ID of the invoice that was created to invoice specifically the costs in this month/period.
		/// If an individual month/period has not been invoiced yet, then this field will not be present until
		/// that invoice is created.
		/// </summary>
		public string InvoiceId { get; set; }
	}
}