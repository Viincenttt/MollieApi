using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.Models;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.SalesInvoice.Response;

namespace Mollie.Api.JsonConverters;

internal class WebhookEventEntityJsonConverter : JsonConverter<object> {
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(IEntity).IsAssignableFrom(typeToConvert);
    }

    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        if (root.ValueKind == JsonValueKind.Object) {
            var enumerator = root.EnumerateObject();
            if (enumerator.MoveNext()) {
                var firstProperty = enumerator.Current;
                if (firstProperty.Value.ValueKind == JsonValueKind.Object) {
                    typeToConvert = GetEntityTypeFromJson(firstProperty, typeToConvert);

                    var arrayJson = firstProperty.Value.GetRawText();
                    return JsonSerializer.Deserialize(arrayJson, typeToConvert, options);
                }
            }
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Not implemented");
    }

    private Type GetEntityTypeFromJson(JsonProperty entityRoot, Type typeToConvert)
    {
        if (typeToConvert != typeof(IEntity)) {
            return typeToConvert;
        }

        if (entityRoot.Value.TryGetProperty("resource", out JsonElement resourceProperty))
        {
            string? resource = resourceProperty.GetString();

            switch (resource) {
                case "payment-link": return typeof(PaymentLinkResponse);
                case "sales-invoice": return typeof(SalesInvoiceResponse);
                default:
                    throw new JsonException(
                        $"Unable to convert embedded JSON to entity type. Resource '{resource}' is not supported or recognized.");
            }
        }

        throw new JsonException("Unable to convert embedded JSON to entity type. No resource property found");
    }
}
