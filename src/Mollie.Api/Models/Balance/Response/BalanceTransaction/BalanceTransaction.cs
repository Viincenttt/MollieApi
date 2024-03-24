using System;

namespace Mollie.Api.Models.Balance.Response.BalanceTransaction {
    public class BalanceTransaction {
        /// <summary>
        /// Indicates the response contains a balance transaction object. Will always contain balance_transaction
        /// for this endpoint.
        /// </summary>
        public required string Resource { get; init; }
        
        /// <summary>
        /// The identifier uniquely referring to this balance transaction. Mollie assigns this identifier at
        /// transaction creation time. For example baltr_QM24QwzUWR4ev4Xfgyt29d.
        /// </summary>
        public required string Id { get; init; }
        
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
        public required DateTime CreatedAt { get; init; }
    }
}