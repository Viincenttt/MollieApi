using System;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public record BalanceReportResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a balance report object. Will always contain balance-report for this endpoint.
        /// </summary>
        public required string Resource { get; init; }
        
        /// <summary>
        /// The ID of a Balance this report is generated for.
        /// </summary>
        public required string BalanceId { get; init; }
        
        /// <summary>
        /// The time zone used for the from and until parameters. Currently only time zone Europe/Amsterdam is supported.
        /// </summary>
        public required string TimeZone { get; init; }
        
        /// <summary>
        /// The start date of the report, in YYYY-MM-DD format. The from date is ‘inclusive’, and in Central European Time.
        /// This means a report with for example from: 2020-01-01 will include movements of 2020-01-01 0:00:00 CET and onwards.
        /// </summary>
        public required DateTime From { get; init; }
        
        /// <summary>
        /// The end date of the report, in YYYY-MM-DD format. The until date is ‘exclusive’, and in Central European Time.
        /// This means a report with for example until: 2020-02-01 will include movements up until 2020-01-31 23:59:59 CET.
        /// </summary>
        public required DateTime Until { get; init; }
        
        /// <summary>
        /// You can retrieve reports in two different formats. With the status-balances format, transactions are grouped by status
        /// (e.g. pending, available), then by direction of movement (e.g. moved from pending to available), then by transaction type,
        /// and then by other sub-groupings where available (e.g. payment method).

        /// With the transaction-categories format, transactions are grouped by transaction type, then by direction of movement, and
        /// then again by other sub-groupings where available.

        /// Both reporting formats will always contain opening and closing amounts that correspond to the start and end dates of the report.
        /// Possible values: status-balances transaction-categories
        /// </summary>
        public required string Grouping { get; init; }
        
        /// <summary>
        /// An object with several URL objects relevant to the balance report.
        /// </summary>
        [JsonProperty("_links")]
        public required BalanceReportLinks Links { get; init; }
    }
}