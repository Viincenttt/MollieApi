namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class ChargebackBalanceTransaction : BalanceTransaction {
        public ChargebackTransactionContext Context { get; set; }
    }
    
    public class ChargebackTransactionContext {
        public string PaymentId { get; set; }
        public string ChargebackId { get; set; }
    }
}