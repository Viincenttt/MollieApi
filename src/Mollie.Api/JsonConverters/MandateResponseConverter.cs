using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Mandate.Response;

namespace Mollie.Api.JsonConverters;


internal class MandateResponseConverter : JsonConverter<MandateResponse>
{
    private readonly MandateResponseFactory _mandateResponseFactory;

    public MandateResponseConverter(MandateResponseFactory mandateResponseFactory)
    {
        _mandateResponseFactory = mandateResponseFactory;
    }

    public override MandateResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse the current JSON object into a JsonDocument for easy querying
        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = document.RootElement;

            // Try to get the "method" property value
            string? paymentMethod = null;
            if (root.TryGetProperty("method", out JsonElement methodProperty))
            {
                paymentMethod = methodProperty.GetString();
            }

            // Use the factory to create the right PaymentResponse instance
            MandateResponse response = _mandateResponseFactory.Create(paymentMethod);

            // Deserialize the JSON into the created instance, using the root element JSON
            // Since System.Text.Json cannot deserialize into existing objects directly,
            // we re-serialize the root JSON and deserialize again into the correct type.

            var json = root.GetRawText();

            // Create new options without this converter to avoid stack overflow
            JsonSerializerOptions newOptions = new(options);
            newOptions.Converters.Remove(this);

            var result = (MandateResponse)JsonSerializer.Deserialize(json, response.GetType(), newOptions)!;

            return result;
        }
    }

    public override void Write(Utf8JsonWriter writer, MandateResponse value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
