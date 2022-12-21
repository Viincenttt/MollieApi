namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class ChargebackTransactionContext : BaseTransactionContext {
        public string PaymentId { get; set; }
        public string ChargebackId { get; set; }
    }
}