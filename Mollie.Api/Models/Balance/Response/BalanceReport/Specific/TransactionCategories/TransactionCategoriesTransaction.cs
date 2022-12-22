namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public class TransactionCategoriesTransaction {
        public BalanceReportAmountWithSubtotals Pending { get; set; }
        public BalanceReportAmountWithSubtotals MovedToAvailable { get; set; }
        public BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; set; }
    }
}