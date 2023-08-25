namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public class StatusBalancesPendingBalance {
        public BalanceReportAmount Open { get; set; }
        public BalanceReportAmount Close { get; set; }
        public BalanceReportAmountWithSubtotals Pending { get; set; }
        public BalanceReportAmountWithSubtotals MovedToAvailable { get; set; }
    }
}