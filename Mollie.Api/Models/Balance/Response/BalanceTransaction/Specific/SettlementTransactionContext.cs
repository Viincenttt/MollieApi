namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class SettlementTransactionContext : BaseTransactionContext {
        public string TransferId { get; set; }
        public string SettlementId { get; set; }
    }
}