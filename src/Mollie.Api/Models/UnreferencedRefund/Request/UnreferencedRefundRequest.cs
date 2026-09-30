using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.UnreferencedRefund.Request;

public record UnreferencedRefundRequest : IProfileRequest {
    /// <summary>
    /// The description of the unreferenced refund. Maximum length of 255 characters.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The amount that you want to refund to the customer on the terminal.
    /// </summary>
    public required Amount Amount { get; set; }

    /// <summary>
    /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the unreferenced
    /// refund. Whenever you fetch the unreferenced refund with our API, we will also include the metadata. You can use up to
    /// approximately 1kB.
    /// </summary>
    [JsonConverter(typeof(RawJsonConverter))]
    public string? Metadata { get; set; }

    /// <summary>
    /// Oauth only - The identifier of the profile the unreferenced refund should be created on. For example pfl_QkEhN94Ba.
    /// </summary>
    public string? ProfileId { get; set; }

    public void SetMetadata(object metadataObj, JsonSerializerOptions? jsonSerializerOptions = null) {
        Metadata = JsonSerializer.Serialize(metadataObj, jsonSerializerOptions);
    }
}
