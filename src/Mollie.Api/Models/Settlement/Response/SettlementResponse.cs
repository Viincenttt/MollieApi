using System;
using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Settlement.Response;

public record SettlementResponse : IEntity {
    /// <summary>
    /// Indicates the response contains a settlement object. Will always contain settlement for this endpoint.
    /// </summary>
    public required string Resource { get; set; }

	/// <summary>
	/// The settlement's identifier, for example stl_jDk30akdN.
	/// </summary>
	public required string Id { get; set; }

	/// <summary>
	/// The settlement's bank reference, as found on your invoice and in your Mollie account.
	/// Not present for the open or next settlement.
	/// </summary>
	public string? Reference { get; set; }

	/// <summary>
	/// The settlement's bank reference, as found in your Mollie account and on your bank statement.
	/// </summary>
	public required DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date on which the settlement was settled.
	/// When requesting the open settlement or next settlement the return value is null.
	/// </summary>
	public DateTime? SettledAt { get; set; }

	/// <summary>
	/// The status of the settlement - See the Mollie.Api.Models.Settlement.SettlementStatus
	/// class for a full list of known values.
	/// </summary>
	public required string Status { get; set; }

	/// <summary>
	/// The total amount paid out with this settlement.
	/// </summary>
	public required Amount Amount { get; set; }

    /// <summary>
    /// The balance token that the settlement was settled to.
    /// </summary>
    public required string BalanceId { get; set; }

    /// <summary>
    /// The ID of the oldest invoice created for all the periods, if the invoice has been created yet.
    /// </summary>
    public required string InvoiceId { get; set; }

	/// <summary>
	/// This object is a collection of Period objects, which describe the settlement by month in full detail.
	/// Please refer to the Period object section below.
	/// </summary>
	[JsonConverter(typeof(SettlementPeriodConverter))]
	public required Dictionary<int, Dictionary<int, SettlementPeriod>> Periods { get; set; }

    /// <summary>
    /// An object with several URL objects relevant to the settlement. Every URL object will contain an href and a type field.
    /// </summary>
    [JsonPropertyName("_links")]
    public required SettlementResponseLinks Links { get; set; }
}
