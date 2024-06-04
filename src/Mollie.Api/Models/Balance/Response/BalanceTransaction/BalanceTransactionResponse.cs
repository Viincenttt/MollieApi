using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Balance.Response.BalanceTransaction {
    public class BalanceTransactionResponse {
        /// <summary>
        /// The number of transactions found in _embedded, which is either the requested number
        /// (with a maximum of 250) or the default number.
        /// </summary>
        public required int Count { get; set; }

        /// <summary>
        /// The object containing the queried data.
        /// </summary>
        [JsonProperty("_embedded")]
        public required BalanceTransactionEmbeddedResponse Embedded { get; set; }

        /// <summary>
        /// Links to help navigate through the lists of balance transactions. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required BalanceTransactionResponseLinks Links { get; set; }
    }

    public class BalanceTransactionEmbeddedResponse {
        [JsonProperty("balance_transactions")]
        public required IEnumerable<BalanceTransaction> BalanceTransactions { get; set; }
    }
}
