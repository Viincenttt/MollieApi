namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public class TransactionCategoriesSummaryBalances {
        public required BalanceReportAmount Pending { get; init; }
        public required BalanceReportAmount Available { get; init; }
    }
}