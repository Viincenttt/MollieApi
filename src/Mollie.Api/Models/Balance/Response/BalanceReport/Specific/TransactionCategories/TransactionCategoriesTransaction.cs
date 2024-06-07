namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public record TransactionCategoriesTransaction {
        public required BalanceReportAmountWithSubtotals Pending { get; set; }
        public required BalanceReportAmountWithSubtotals MovedToAvailable { get; set; }
        public required BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; set; }
    }
}
