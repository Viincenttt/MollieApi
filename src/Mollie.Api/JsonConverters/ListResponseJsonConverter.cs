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

            if (root.ValueKind == JsonValueKind.Object) {
                var enumerator = root.EnumerateObject();
                if (enumerator.MoveNext()) {
                    var firstProperty = enumerator.Current;
                    if (firstProperty.Value.ValueKind == JsonValueKind.Array) {
                        var arrayJson = firstProperty.Value.GetRawText();
                        return JsonSerializer.Deserialize(arrayJson, typeToConvert, options);
                    }
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
