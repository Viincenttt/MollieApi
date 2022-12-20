using System.Collections.Generic;

namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public class BalanceReportSubtotals {
        public string TransactionType { get; set; }
        public string Method { get; set; }
        public string PrepaymentPartType { get; set; }
        public string FeeType { get; set; }
        public int Count { get; set; }
        public Amount Amount { get; set; }
        public IEnumerable<BalanceReportSubtotals> Subtotals { get; set; }
    }
}