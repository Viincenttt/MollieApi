using System.Collections.Generic;

namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public record BalanceReportSubtotals {
        public string? TransactionType { get; set; }
        public string? Method { get; set; }
        public string? PrepaymentPartType { get; set; }
        public required string FeeType { get; set; }
        public required int Count { get; set; }
        public required Amount Amount { get; set; }
        public IEnumerable<BalanceReportSubtotals>? Subtotals { get; set; }
    }
}
