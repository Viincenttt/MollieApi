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
	}
}