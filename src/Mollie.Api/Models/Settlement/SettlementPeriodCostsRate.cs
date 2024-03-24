namespace Mollie.Api.Models.Settlement
{
	public class SettlementPeriodCostsRate {
        /// <summary>
        /// An amount object describing the fixed costs.
        /// </summary>
		public required Amount Fixed { get; init; }

        /// <summary>
        /// A string describing the variable costs as a percentage.
        /// </summary>
		public required string Percentage { get; init; }
	}
}
