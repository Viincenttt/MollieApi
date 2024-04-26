namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public record TransactionCategoriesReportResponse : BalanceReportResponse {
        public required TransactionCategoriesTotal Totals { get; init; }
    }
}