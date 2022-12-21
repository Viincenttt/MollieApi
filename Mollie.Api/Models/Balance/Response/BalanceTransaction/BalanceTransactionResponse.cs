using System;
using System.Collections.Generic;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Balance.Response.BalanceTransaction {
    public class BalanceTransactionResponse {
        /// <summary>
        /// The number of transactions found in _embedded, which is either the requested number
        /// (with a maximum of 250) or the default number.
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// The object containing the queried data.
        /// </summary>
        [JsonProperty("_embedded")]
        public BalanceTransactionEmbeddedResponse Embedded { get; set; }
    }

    public class BalanceTransactionEmbeddedResponse {
        [JsonProperty("balance_transactions")]
        public IEnumerable<BalanceTransaction> BalanceTransactions { get; set; }
    }

    public class BalanceTransaction {
        /// <summary>
        /// Indicates the response contains a balance transaction object. Will always contain balance_transaction
        /// for this endpoint.
        /// </summary>
        public string Resource { get; set; }
        
        /// <summary>
        /// The identifier uniquely referring to this balance transaction. Mollie assigns this identifier at
        /// transaction creation time. For example baltr_QM24QwzUWR4ev4Xfgyt29d.
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The type of movement, for example payment or refund. See Mollie docs for a full list of values
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// The final amount that was moved to or from the balance, e.g. {"currency":"EUR", "value":"100.00"}.
        /// If the transaction moves funds away from the balance, for example when it concerns a refund, the
        /// amount will be negative.
        /// </summary>
        public Amount ResultAmount { get; set; }
        
        /// <summary>
        /// The amount that was to be moved to or from the balance, excluding deductions. If the transaction
        /// moves funds away from the balance, for example when it concerns a refund, the amount will be negative.
        /// </summary>
        public Amount InitialAmount { get; set; }
        
        /// <summary>
        /// The total amount of deductions withheld from the movement. For example, if a €10,00 payment comes in
        /// with a €0,29 fee, the deductions amount will be {"currency":"EUR", "value":"-0.29"}. When moving funds
        /// to a balance, we always round the deduction to a ‘real’ amount. Any differences between these realtime
        /// rounded amounts and the final invoice will be compensated when the invoice is generated.
        /// </summary>
        public Amount Deductions { get; set; }
        
        /// <summary>
        /// The date and time of the movement, in ISO 8601 format.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Depending on the type of the balance transaction, we will try to give more context about the specific event
        /// that triggered the movement. 
        /// </summary>
        public BaseTransactionContext Context { get; set; }
    }

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