namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public class StatusBalanceAvailableBalance {
        public BalanceReportAmount Open { get; set; }
        public BalanceReportAmount Close { get; set; }
        public BalanceReportAmountWithSubtotals MovedFromPending { get; set; }
        public BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; set; }
    }
}