namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public record StatusBalanceReportResponse : BalanceReportResponse {
        public required StatusBalancesTotal Totals { get; set; }
    }
}
