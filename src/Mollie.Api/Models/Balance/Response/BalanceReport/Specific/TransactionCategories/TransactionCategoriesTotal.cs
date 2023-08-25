using Newtonsoft.Json;

namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
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
}