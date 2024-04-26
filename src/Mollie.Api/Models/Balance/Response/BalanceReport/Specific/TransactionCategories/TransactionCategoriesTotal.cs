using Newtonsoft.Json;

namespace Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories {
    public class TransactionCategoriesTotal {
        public required TransactionCategoriesSummaryBalances Open { get; init; }
        public required TransactionCategoriesSummaryBalances Close { get; init; }
        public required TransactionCategoriesTransaction Payments { get; init; }
        public required TransactionCategoriesTransaction Refunds { get; init; }
        public required TransactionCategoriesTransaction Chargebacks { get; init; }
        public required TransactionCategoriesTransaction Capital { get; init; }
        public required TransactionCategoriesTransaction Transfers { get; init; }
        [JsonProperty("fee-prepayments")]
        public required TransactionCategoriesTransaction FeePrepayments { get; init; }
        public required TransactionCategoriesTransaction Corrections { get; init; }
    }
}