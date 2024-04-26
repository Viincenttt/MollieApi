namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public class StatusBalanceReportResponse : BalanceReportResponse {
        public required StatusBalancesTotal Totals { get; init; }
    }
}