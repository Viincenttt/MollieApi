namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class SettlementBalanceTransaction : BalanceTransaction {
        public SettlementTransactionContext Context { get; set; }
    }
    
    public class SettlementTransactionContext {
        public string TransferId { get; set; }
        public string SettlementId { get; set; }
    }
}