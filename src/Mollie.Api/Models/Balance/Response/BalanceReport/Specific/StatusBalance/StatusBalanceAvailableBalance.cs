namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public class StatusBalanceAvailableBalance {
        public required BalanceReportAmount Open { get; init; }
        public required BalanceReportAmount Close { get; init; }
        public required BalanceReportAmountWithSubtotals MovedFromPending { get; init; }
        public required BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; init; }
    }
}