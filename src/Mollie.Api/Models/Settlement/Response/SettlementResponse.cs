using System;
using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Settlement.Response {
	public record SettlementResponse {
        /// <summary>
        /// Indicates the response contains a settlement object. Will always contain settlement for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

		/// <summary>
		/// The settlement's identifier, for example stl_jDk30akdN.
		/// </summary>
		public required string Id { get; init; }

		/// <summary>
		/// The settlement's bank reference, as found on your invoice and in your Mollie account.
		/// </summary>
		public required string Reference { get; init; }

		/// <summary>
		/// The date on which the settlement was created.
		/// When requesting the next settlement the returned date signifies the expected settlement date.
		/// </summary>
		public required DateTime CreatedAt { get; init; }

		/// <summary>
		/// The date on which the settlement was settled.
		/// When requesting the open settlement or next settlement the return value is null.
		/// </summary>
		public DateTime? SettledAt { get; set; }

		/// <summary>
		/// The status of the settlement - See the Mollie.Api.Models.Settlement.SettlementStatus
		/// class for a full list of known values.
		/// </summary>
		public required string Status { get; init; }

		/// <summary>
		/// The total amount paid out with this settlement.
		/// </summary>
		public required Amount Amount { get; init; }

		/// <summary>
		/// This object is a collection of Period objects, which describe the settlement by month in full detail.
		/// Please refer to the Period object section below.
		/// </summary>
		[JsonConverter(typeof(SettlementPeriodConverter))]
		public required Dictionary<int, Dictionary<int, SettlementPeriod>> Periods { get; init; }

        /// <summary>
        /// An object with several URL objects relevant to the settlement. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required SettlementResponseLinks Links { get; init; }
    }
}
