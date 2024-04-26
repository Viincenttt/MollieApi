namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public record TransactionCategoriesTransaction {
        public required BalanceReportAmountWithSubtotals Pending { get; init; }
        public required BalanceReportAmountWithSubtotals MovedToAvailable { get; init; }
        public required BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; init; }
    }
}