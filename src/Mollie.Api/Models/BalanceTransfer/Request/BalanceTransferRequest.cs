using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.BalanceTransfer.Request;

public record BalanceTransferRequest : ITestModeRequest {
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
    /// The type of the transfer. Different fees may apply to different types of transfers.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// A JSON object that you can attach to a balance transfer. This can be useful for storing additional
    /// information about the transfer in a structured format. Maximum size is approximately 1KB.
    /// </summary>
    [JsonConverter(typeof(RawJsonConverter))]
    public string? Metadata { get; set; }

    /// <summary>
    /// Whether to create the entity in test mode or live mode. You can enable test mode by setting testmode to true.
    /// </summary>
    public bool? Testmode { get; set; }

    public void SetMetadata(object metadataObj, JsonSerializerOptions? jsonSerializerOptions = null) {
        Metadata = JsonSerializer.Serialize(metadataObj, jsonSerializerOptions);
    }
}
