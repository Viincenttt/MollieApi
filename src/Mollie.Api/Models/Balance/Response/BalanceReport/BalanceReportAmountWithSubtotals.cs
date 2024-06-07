using System.Collections.Generic;

namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public record BalanceReportAmountWithSubtotals : BalanceReportAmount {
        public required IEnumerable<BalanceReportSubtotals> Subtotals { get; set; }
    }
}
