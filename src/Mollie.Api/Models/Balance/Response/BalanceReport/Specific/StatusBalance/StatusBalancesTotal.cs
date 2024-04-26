namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance {
    public record StatusBalancesTotal {
        public required StatusBalancesPendingBalance PendingBalance { get; init; }
        public required StatusBalanceAvailableBalance AvailableBalance { get; init; }
    }
}