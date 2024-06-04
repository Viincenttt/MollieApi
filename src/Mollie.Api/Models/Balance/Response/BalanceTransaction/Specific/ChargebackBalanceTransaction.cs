namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class ChargebackBalanceTransaction : BalanceTransaction {
        public required ChargebackTransactionContext Context { get; set; }
    }

    public class ChargebackTransactionContext {
        public required string PaymentId { get; set; }
        public required string ChargebackId { get; set; }
    }
}
