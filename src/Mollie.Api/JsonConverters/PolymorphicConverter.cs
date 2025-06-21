using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.Framework.Factories;

namespace Mollie.Api.JsonConverters;

internal class PolymorphicConverter<T> : JsonConverter<T> {
    private readonly ITypeFactory<T> _typeFactory;
    private readonly string _typeFieldName;

    public PolymorphicConverter(ITypeFactory<T> typeFactory, string typeFieldName)
    {
        _typeFactory = typeFactory;
        _typeFieldName = typeFieldName;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        // Parse the current JSON object into a JsonDocument for easy querying
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        // Try to get the "method" property value
        string? type = null;
        if (root.TryGetProperty(_typeFieldName, out JsonElement typeProperty))
        {
            type = typeProperty.GetString();
        }

        // Use the factory to create the right PaymentResponse instance
        T response = _typeFactory.Create(type);

        // Deserialize the JSON into the created instance, using the root element JSON
        // Since System.Text.Json cannot deserialize into existing objects directly,
        // we re-serialize the root JSON and deserialize again into the correct type.

        var json = root.GetRawText();

        // Create new options without this converter to avoid stack overflow
        JsonSerializerOptions newOptions = new(options);
        newOptions.Converters.Remove(this);

        var result = (T)JsonSerializer.Deserialize(json, response.GetType(), newOptions)!;

        return result;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

