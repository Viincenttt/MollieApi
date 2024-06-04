namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class InvoiceBalanceTransaction : BalanceTransaction {
        public required InvoiceTransactionContext Context { get; set; }
    }

    public class InvoiceTransactionContext {
        public required string InvoiceId { get; set; }
    }
}
