namespace Mollie.Api.Models.Balance.Response.Specific.TransactionCategories {
    public class TransactionCategoriesReportResponse : BalanceReportResponse {
        public TransactionCategoriesReportTotalsResponse Totals { get; set; }
    }
    
    public class TransactionCategoriesReportTotalsResponse {
        public TransactionCategoriesReportTotalsOpenResponse Open { get; set; }
    }

    public class TransactionCategoriesReportTotalsOpenResponse {
        public TransactionCategoriesReportAmountResponse Pending { get; set; }
        public TransactionCategoriesReportAmountResponse Available { get; set; }
    }

    public class TransactionCategoriesReportAmountResponse {
        public Amount Amount { get; set; }
    }

    public class TransactionCategoriesPaymentsResponse {
        public TransactionCategoriesReportAmountResponse Pending { get; set; }
    }
}