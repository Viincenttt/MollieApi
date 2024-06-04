﻿namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class RefundBalanceTransaction : BalanceTransaction {
        public required RefundTransactionContext Context { get; set; }
    }

    public class RefundTransactionContext {
        public required string PaymentId { get; set; }
        public required string RefundId { get; set; }
    }
}
