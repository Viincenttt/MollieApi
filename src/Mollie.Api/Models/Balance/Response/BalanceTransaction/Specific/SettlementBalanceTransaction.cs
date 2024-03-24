namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class SettlementBalanceTransaction : BalanceTransaction {
        public required SettlementTransactionContext Context { get; init; }
    }
    
    public class SettlementTransactionContext {
        public required string TransferId { get; init; }
        public required string SettlementId { get; init; }
    }
}