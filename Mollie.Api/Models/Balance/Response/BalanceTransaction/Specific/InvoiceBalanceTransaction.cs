namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class InvoiceBalanceTransaction : BalanceTransaction {
        public InvoiceTransactionContext Context { get; set; }
    }
    
    public class InvoiceTransactionContext {
        public string InvoiceId { get; set; }
    }
}