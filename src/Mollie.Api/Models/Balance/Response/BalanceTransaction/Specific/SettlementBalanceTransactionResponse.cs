namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class SettlementBalanceTransactionResponse : BalanceTransactionResponse {
        public required SettlementTransactionContext Context { get; set; }
    }

    public class SettlementTransactionContext {
        public required string TransferId { get; set; }
        public required string SettlementId { get; set; }
    }
}
