namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public record StatusBalancesPendingBalance {
        public required BalanceReportAmount Open { get; set; }
        public required BalanceReportAmount Close { get; set; }
        public required BalanceReportAmountWithSubtotals Pending { get; set; }
        public required BalanceReportAmountWithSubtotals MovedToAvailable { get; set; }
    }
}
