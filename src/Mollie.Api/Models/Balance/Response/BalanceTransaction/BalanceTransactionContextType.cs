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
        public const string CaptureCommission = "capture-commission";
        public const string CaptureRollingReserveRelease = "capture-rolling-reserve-release";
        public const string RefundCompensation = "refund-compensation";
        public const string ReturnedRefundCompensation = "returned-refund-compensation";
        public const string ChargebackCompensation = "chargeback-compensation";
        public const string ReversedChargebackCompensation = "reversed-chargeback-compensation";
        public const string ReturnedPlatformPaymentRefund = "returned-platform-payment-refund";
        public const string ReversedPlatformPaymentChargeback = "reversed-platform-payment-chargeback";
        public const string PaymentCommission = "payment-commission";
        public const string ReimbursementFee = "reimbursement-fee";
        public const string FailedPaymentFee = "failed-payment-fee";
        public const string PaymentFee = "payment-fee";
        public const string ManagedFee = "managed-fee";
        public const string ReturnedManagedFee = "returned-managed-fee";
        public const string PostPaymentSplitPayment = "post-payment-split-payment";
    }
}