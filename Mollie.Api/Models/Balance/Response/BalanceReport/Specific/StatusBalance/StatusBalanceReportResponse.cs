namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public class StatusBalanceReportResponse : BalanceReportResponse {
        public StatusBalancesTotal Totals { get; set; }
    }
    
    public class StatusBalancesTotal {
        public StatusBalancesPendingBalance PendingBalance { get; set; }
        public StatusBalanceAvailableBalance AvailableBalance { get; set; }
    }

    public class StatusBalancesPendingBalance {
        public BalanceReportAmount Open { get; set; }
        public BalanceReportAmount Close { get; set; }
        public BalanceReportAmountWithSubtotals Pending { get; set; }
        public BalanceReportAmountWithSubtotals MovedToAvailable { get; set; }
    }

    public class StatusBalanceAvailableBalance {
        public BalanceReportAmount Open { get; set; }
        public BalanceReportAmount Close { get; set; }
        public BalanceReportAmountWithSubtotals MovedFromPending { get; set; }
        public BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; set; }
    }
}