using System.Collections.Generic;
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
        public TransactionCategoriesAmount Pending { get; set; }
        public TransactionCategoriesAmount Available { get; set; }
    }
    
    public class TransactionCategoriesTransaction {
        public TransactionCategoriesAmountWithSubtotals Pending { get; set; }
        public TransactionCategoriesAmountWithSubtotals MovedToAvailable { get; set; }
        public TransactionCategoriesAmountWithSubtotals ImmediatelyAvailable { get; set; }
    }

    public class TransactionCategoriesAmount {
        public Amount Amount { get; set; }

        public override string ToString() {
            return this.Amount.ToString();
        }
    }

    public class TransactionCategoriesAmountWithSubtotals : TransactionCategoriesAmount {
        public IEnumerable<TransactionCategoriesSubtotals> Subtotals { get; set; }
    }
    
    public class TransactionCategoriesSubtotals {
        public string TransactionType { get; set; }
        public string Method { get; set; }
        public string PrepaymentPartType { get; set; }
        public string FeeType { get; set; }
        public int Count { get; set; }
        public Amount Amount { get; set; }
        public IEnumerable<TransactionCategoriesSubtotals> Subtotals { get; set; }
    }
}