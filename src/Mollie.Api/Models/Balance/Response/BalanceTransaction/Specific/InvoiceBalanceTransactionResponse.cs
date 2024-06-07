namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class InvoiceBalanceTransactionResponse : BalanceTransactionResponse {
        public required InvoiceTransactionContext Context { get; set; }
    }

    public class InvoiceTransactionContext {
        public required string InvoiceId { get; set; }
    }
}
