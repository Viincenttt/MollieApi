using System.Collections.Generic;

namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public record BalanceReportSubtotals {
        public string? TransactionType { get; set; }
        public string? Method { get; set; }
        public string? PrepaymentPartType { get; init; }
        public required string FeeType { get; init; }
        public required int Count { get; init; }
        public required Amount Amount { get; init; }
        public IEnumerable<BalanceReportSubtotals>? Subtotals { get; set; }
    }
}