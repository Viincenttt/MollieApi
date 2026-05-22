using System;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Payout.Response;

public record PayoutResponse {
    /// <summary>
    /// Indicates the response contains a payout object. Will always contain the string payout for this endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// The identifier uniquely referring to this payout.
    /// Example: payout_j8NvRAM2WNZtsykpLEX8J.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The identifier of the balance that will be paid out.
    /// Example: bal_gVMhHKqSSRYJyPsuoPNFH.
    /// </summary>
    public required string BalanceId { get; set; }

    /// <summary>
    /// The amount to pay out. The value reflects the amount paid out, excluding any applicable fees.
    /// </summary>
    public Amount? Amount { get; set; }

    /// <summary>
    /// The description that will appear on the bank statement for this payout.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The status of the payout.
    /// Refer to the <see cref="PayoutStatus"/> class for a full list of known values.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The reason for the payout's current status.
    /// </summary>
    public required StatusReason StatusReason { get; set; }

    /// <summary>
    /// The entity's date and time of creation, in ISO 8601 format.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date and time the payout was initiated, in ISO 8601 format.
    /// null if the payout has not been initiated yet.
    /// </summary>
    public DateTime? InitiatedAt { get; set; }

    /// <summary>
    /// The date and time the payout was sent to the destination bank account, in ISO 8601 format.
    /// null if the payout has not completed yet.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// The date and time the payout was canceled, in ISO 8601 format.
    /// null if the payout was not canceled.
    /// </summary>
    public DateTime? CanceledAt { get; set; }

    /// <summary>
    /// Whether this entity was created in live mode or in test mode.
    /// </summary>
    public required Mode Mode { get; set; }

    /// <summary>
    /// An object with several relevant URLs.
    /// </summary>
    [JsonPropertyName("_links")]
    public required PayoutResponseLinks Links { get; set; }
}



