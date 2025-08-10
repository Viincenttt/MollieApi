using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.Models;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.Invoice.Response;
using Mollie.Api.Models.Issuer.Response;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.SalesInvoice.Response;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Shipment.Response;
using Mollie.Api.Models.Subscription.Response;
using Mollie.Api.Models.Terminal.Response;

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
                case "balance": return typeof(BalanceResponse);
                case "capture": return typeof(CaptureResponse);
                case "chargeback": return typeof(ChargebackResponse);
                case "customer": return typeof(CustomerResponse);
                case "invoice": return typeof(InvoiceResponse);
                case "issuer": return typeof(IssuerResponse);
                case "mandate": return typeof(MandateResponse);
                case "order": return typeof(OrderResponse);
                case "refund": return typeof(RefundResponse);
                case "settlement": return typeof(SettlementResponse);
                case "shipment": return typeof(ShipmentResponse);
                case "subscription": return typeof(SubscriptionResponse);
                case "terminal": return typeof(TerminalResponse);
                case "payment": return typeof(PaymentResponse);
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
