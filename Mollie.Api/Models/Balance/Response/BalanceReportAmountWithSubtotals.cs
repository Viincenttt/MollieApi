using System.Collections.Generic;

namespace Mollie.Api.Models.Balance.Response {
    public class BalanceReportAmountWithSubtotals : BalanceReportAmount {
        public IEnumerable<BalanceReportSubtotals> Subtotals { get; set; }
    }
}