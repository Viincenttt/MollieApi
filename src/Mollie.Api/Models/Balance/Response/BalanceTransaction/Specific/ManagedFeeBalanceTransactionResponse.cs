namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class ManagedFeeBalanceTransactionResponse : BalanceTransactionResponse {
        public required ManagedFeeTransactionContext Context { get; set; }
    }

    public class ManagedFeeTransactionContext {
        /// <summary>
        /// The type of the managed fee.
        /// </summary>
        public required string FeeType { get; set; }

        /// <summary>
        /// The identifier of the fee.
        /// </summary>
        public string? FeeId { get; set; }
    }
}
