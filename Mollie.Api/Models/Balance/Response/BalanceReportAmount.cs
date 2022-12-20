namespace Mollie.Api.Models.Balance.Response {
    public class BalanceReportAmount {
        public Amount Amount { get; set; }
        
        public override string ToString() {
            return this.Amount.ToString();
        }
    }
}