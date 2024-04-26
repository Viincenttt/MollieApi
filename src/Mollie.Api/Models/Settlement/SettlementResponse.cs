﻿using System;
using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Settlement {
	public class SettlementResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a settlement object. Will always contain settlement for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

		/// <summary>
		/// The settlement's identifier, for example stl_jDk30akdN.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The settlement's bank reference, as found on your invoice and in your Mollie account.
		/// </summary>
		public string Reference { get; set; }

		/// <summary>
		/// The date on which the settlement was created.
		/// When requesting the next settlement the returned date signifies the expected settlement date.
		/// When requesting the open settlement (open funds) the return value is null.
		/// </summary>
		public DateTime? CreatedAt { get; set; }

		/// <summary>
		/// The date on which the settlement was settled.
		/// When requesting the open settlement or next settlement the return value is null.
		/// </summary>
		public DateTime? SettledAt { get; set; }

		/// <summary>
		/// The status of the settlement - See the Mollie.Api.Models.Settlement.SettlementStatus 
		/// class for a full list of known values.
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// The total amount paid out with this settlement.
		/// </summary>
		public Amount Amount { get; set; }

		/// <summary>
		/// This object is a collection of Period objects, which describe the settlement by month in full detail.
		/// Please refer to the Period object section below.
		/// </summary>
		[JsonConverter(typeof(SettlementPeriodConverter))]
		public Dictionary<int, Dictionary<int, SettlementPeriod>> Periods { get; set; }

		/// <summary>
		/// A list of all payment IDs that make up the settlement. You can use this to fully reconciliate the settlement with your back office.
		/// </summary>
		public List<string> PaymentIds { get; set; }

		/// <summary>
		/// A list of all refund IDs that make up the settlement. You can use this to fully reconciliate the settlement with your back office.
		/// </summary>
		public List<string> RefundIds { get; set; }

		/// <summary>
		/// A list of all chargeback IDs that make up the settlement. You can use this to fully reconciliate the settlement with your back office.
		/// </summary>
		public List<string> ChargebackIds { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the settlement. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public SettlementResponseLinks Links { get; set; }
    }
}