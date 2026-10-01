namespace Mollie.Api.Models.Balance.Response.BalanceTransaction {
    public record BalanceTransactionDeductionDetails {
        /// <summary>
        /// The amount deducted for all transaction fees.
        /// </summary>
        public Amount? Fees { get; set; }

        /// <summary>
        /// The amount deducted for commissions (e.g. application fees).
        /// </summary>
        public Amount? Commissions { get; set; }

        /// <summary>
        /// The amount deducted for Mollie Capital repayments.
        /// </summary>
        public Amount? Repayments { get; set; }

        /// <summary>
        /// The amount deducted for rolling reservations.
        /// </summary>
        public Amount? Reservations { get; set; }
    }
}
