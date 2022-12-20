using Newtonsoft.Json;

namespace Mollie.Api.Models.Balance.Response.Specific.TransactionCategories {
    public class TransactionCategoriesReportResponse : BalanceReportResponse {
        public TransactionCategoriesTotal Totals { get; set; }
    }
    
    public class TransactionCategoriesTotal {
        public TransactionCategoriesSummaryBalances Open { get; set; }
        public TransactionCategoriesSummaryBalances Close { get; set; }
        public TransactionCategoriesTransaction Payments { get; set; }
        public TransactionCategoriesTransaction Refunds { get; set; }
        public TransactionCategoriesTransaction Chargebacks { get; set; }
        public TransactionCategoriesTransaction Capital { get; set; }
        public TransactionCategoriesTransaction Transfers { get; set; }
        [JsonProperty("fee-prepayments")]
        public TransactionCategoriesTransaction FeePrepayments { get; set; }
        public TransactionCategoriesTransaction Corrections { get; set; }
    }

    public class TransactionCategoriesSummaryBalances {
        public BalanceReportAmount Pending { get; set; }
        public BalanceReportAmount Available { get; set; }
    }
    
    public class TransactionCategoriesTransaction {
        public BalanceReportAmountWithSubtotals Pending { get; set; }
        public BalanceReportAmountWithSubtotals MovedToAvailable { get; set; }
        public BalanceReportAmountWithSubtotals ImmediatelyAvailable { get; set; }
    }
}