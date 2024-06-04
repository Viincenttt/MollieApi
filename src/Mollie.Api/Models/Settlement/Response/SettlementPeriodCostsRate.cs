﻿namespace Mollie.Api.Models.Settlement.Response
{
	public record SettlementPeriodCostsRate {
        /// <summary>
        /// An amount object describing the fixed costs.
        /// </summary>
		public required Amount Fixed { get; set; }

        /// <summary>
        /// A string describing the variable costs as a percentage.
        /// </summary>
		public required string Percentage { get; set; }
	}
}
