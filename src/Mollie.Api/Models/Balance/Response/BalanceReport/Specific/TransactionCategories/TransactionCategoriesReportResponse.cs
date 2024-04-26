namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public class TransactionCategoriesReportResponse : BalanceReportResponse {
        public required TransactionCategoriesTotal Totals { get; init; }
    }
}