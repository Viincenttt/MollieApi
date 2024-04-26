namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public class StatusBalancesPendingBalance {
        public required BalanceReportAmount Open { get; init; }
        public required BalanceReportAmount Close { get; init; }
        public required BalanceReportAmountWithSubtotals Pending { get; init; }
        public required BalanceReportAmountWithSubtotals MovedToAvailable { get; init; }
    }
}