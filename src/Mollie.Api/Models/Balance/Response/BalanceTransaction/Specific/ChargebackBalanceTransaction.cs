namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class ChargebackBalanceTransaction : BalanceTransaction {
        public required ChargebackTransactionContext Context { get; init; }
    }
    
    public class ChargebackTransactionContext {
        public required string PaymentId { get; init; }
        public required string ChargebackId { get; init; }
    }
}