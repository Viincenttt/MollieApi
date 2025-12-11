using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.BalanceTransfer.Response;

public record BalanceTransferResponse {
    /// <summary>
    /// Indicates the response contains a balance transfer object. Will always contain the string
    /// connect-balance-transfer for this endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// The identifier uniquely referring to this balance transfer. Mollie assigns this identifier at balance transfer
    /// creation time. Mollie will always refer to the balance transfer by this ID. Example: cbtr_j8NvRAM2WNZtsykpLEX8J.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The amount to be transferred, e.g. {"currency":"EUR", "value":"1000.00"} if you would like to transfer €1000.00.
    /// </summary>
    public required Amount Amount { get; set; }

    /// <summary>
    /// A party involved in the balance transfer, either the sender or the receiver.
    /// </summary>
    public required BalanceTransferParty Source { get; set; }

    /// <summary>
    /// A party involved in the balance transfer, either the sender or the receiver.
    /// </summary>
    public required BalanceTransferParty Destination { get; set; }

    /// <summary>
    /// The transfer description for initiating party.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The status of the transfer.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The type of the transfer. Different fees may apply to different types of transfers.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// A JSON object that you can attach to a balance transfer. This can be useful for storing additional
    /// information about the transfer in a structured format. Maximum size is approximately 1KB.
    /// </summary>
    [JsonConverter(typeof(RawJsonConverter))]
    public string? Metadata { get; set; }

    public void SetMetadata(object metadataObj, JsonSerializerOptions? jsonSerializerOptions = null) {
        Metadata = JsonSerializer.Serialize(metadataObj, jsonSerializerOptions);
    }
}
