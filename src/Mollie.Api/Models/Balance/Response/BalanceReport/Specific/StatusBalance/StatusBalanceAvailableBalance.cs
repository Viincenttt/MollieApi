namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public record StatusBalanceAvailableBalance {
        public required BalanceReportAmount Open { get; set; }
        public required BalanceReportAmount Close { get; set; }
        public required BalanceReportAmountWithSubtotals MovedFromPending { get; set; }
        public required BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; set; }
    }
}
