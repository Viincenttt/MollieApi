using System.Collections.Generic;

namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public class BalanceReportAmountWithSubtotals : BalanceReportAmount {
        public IEnumerable<BalanceReportSubtotals> Subtotals { get; set; }
    }
}