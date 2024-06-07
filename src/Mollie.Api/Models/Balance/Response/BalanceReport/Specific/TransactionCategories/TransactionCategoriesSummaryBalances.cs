namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public record TransactionCategoriesSummaryBalances {
        public required BalanceReportAmount Pending { get; set; }
        public required BalanceReportAmount Available { get; set; }
    }
}
