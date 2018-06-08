namespace Mollie.Api.Models.Settlement
{
	public class SettlementPeriodCostsRate {
        /// <summary>
        /// An amount object describing the fixed costs.
        /// </summary>
		public Amount Fixed { get; set; }

        /// <summary>
        /// A string describing the variable costs as a percentage.
        /// </summary>
		public string Variable { get; set; }
	}
}