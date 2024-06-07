using Newtonsoft.Json;

namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public record TransactionCategoriesTotal {
        public required TransactionCategoriesSummaryBalances Open { get; set; }
        public required TransactionCategoriesSummaryBalances Close { get; set; }
        public required TransactionCategoriesTransaction Payments { get; set; }
        public required TransactionCategoriesTransaction Refunds { get; set; }
        public required TransactionCategoriesTransaction Chargebacks { get; set; }
        public required TransactionCategoriesTransaction Capital { get; set; }
        public required TransactionCategoriesTransaction Transfers { get; set; }
        [JsonProperty("fee-prepayments")]
        public required TransactionCategoriesTransaction FeePrepayments { get; set; }
        public required TransactionCategoriesTransaction Corrections { get; set; }
    }
}
