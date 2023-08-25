namespace Mollie.Api.Models.Balance.Response.BalanceTransaction {
    public static class BalanceTransactionContextType {
        public const string Payment = "payment";
        public const string Capture = "capture";
        public const string UnauthorizedDirectDebit = "unauthorized-direct-debit";
        public const string FailedPayment = "failed-payment";
        public const string Refund = "refund";
        public const string ReturnedRefund = "returned-refund";
        public const string Chargeback = "chargeback";
        public const string ChargebackReversal = "chargeback-reversal";
        public const string OutgoingTransfer = "outgoing-transfer";
        public const string CanceledOutgoingTransfer = "canceled-outgoing-transfer";
        public const string ReturnedTransfer = "returned-transfer";
        public const string InvoiceCompensation = "invoice-compensation";
        public const string BalanceCorrection = "balance-correction";
        public const string ApplicationFee = "application-fee";
        public const string SplitPayment = "split-payment";
        public const string PlatformPaymentRefund = "platform-payment-refund";
        public const string PlatformPaymentChargeback = "platform-payment-chargeback";
    }
}