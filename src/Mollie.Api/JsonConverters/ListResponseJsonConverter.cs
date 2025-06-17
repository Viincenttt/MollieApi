using System;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters;

internal class ListResponseConverter : JsonConverter<object>
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Check if the target type is assignable from IList (List<T> implements IList)
        return typeof(IList).IsAssignableFrom(typeToConvert);
    }

    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = document.RootElement;

            if (root.ValueKind == JsonValueKind.Object)
            {
                // TODO: This doesn't work
                var test = root.EnumerateObject().Current;
                 root.EnumerateObject().MoveNext();
                 var firstProperty = root.EnumerateObject().Current;

                if (firstProperty.Value.ValueKind == JsonValueKind.Array)
                {
                    string json = firstProperty.Value.GetRawText();

                    // Deserialize the array JSON into the target list type
                    return JsonSerializer.Deserialize(json, typeToConvert, options);
                }
            }
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Not implemented");
    }
}
