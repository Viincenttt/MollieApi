namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public record BalanceReportAmount {
        public required Amount Amount { get; set; }

        public override string ToString() {
            return Amount.ToString();
        }
    }
}
