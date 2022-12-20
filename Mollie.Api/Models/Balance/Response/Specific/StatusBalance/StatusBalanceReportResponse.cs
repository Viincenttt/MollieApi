namespace Mollie.Api.Models.Balance.Response.Specific.StatusBalance {
    public class StatusBalanceReportResponse : BalanceReportResponse {
        public StatusBalancesTotal Totals { get; set; }
    }
    
    public class StatusBalancesTotal {
        public StatusBalancesPendingBalance PendingBalance { get; set; }
        public StatusBalanceAvailableBalance AvailableBalance { get; set; }
    }

    public class StatusBalancesPendingBalance {
        public StatusBalanceAmount Open { get; set; }
        public StatusBalanceAmount Pending { get; set; }
        public StatusBalanceAmount MovedToAvailable { get; set; }
        public StatusBalanceAmount Close { get; set; }
    }

    public class StatusBalanceAvailableBalance {
        public StatusBalanceAmount Open { get; set; }
        public StatusBalanceAmount MovedFromPending { get; set; }
        public StatusBalanceAmount ImmediatelyAvailable { get; set; }
        public StatusBalanceAmount Close { get; set; }
    }

    public class StatusBalanceAmount {
        public Amount Amount { get; set; }
        
        public override string ToString() {
            return this.Amount.ToString();
        }
    }
}