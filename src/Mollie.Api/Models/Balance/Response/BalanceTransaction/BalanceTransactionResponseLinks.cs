﻿using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Balance.Response.BalanceTransaction {
    public class BalanceTransactionResponseLinks {
        /// <summary>
        /// The URL to the current set of balance transactions.
        /// </summary>
        public required UrlObjectLink<BalanceTransactionResponse> Self { get; set; }

        /// <summary>
        /// The previous set of balance transactions, if available.
        /// </summary>
        public required UrlLink Previous { get; set; }

        /// <summary>
        /// The next set of balance transactions, if available.
        /// </summary>
        public required UrlLink Next { get; set; }

        /// <summary>
        /// The URL to the balance transactions list endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
