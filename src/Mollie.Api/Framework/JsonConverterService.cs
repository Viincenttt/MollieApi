using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Mollie.Api.Framework.Factories;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Framework;

internal class JsonConverterService
{
    private readonly JsonSerializerOptions _defaultJsonDeserializerOptions;

    public JsonConverterService()
    {
        _defaultJsonDeserializerOptions = CreateDefaultJsonDeserializerOptions();
    }

    public string Serialize(object objectToSerialize)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
            AllowTrailingCommas = true,
            RespectNullableAnnotations = false
        };
        options.Converters.Add(new JsonDateConverter("yyyy-MM-dd"));
        return JsonSerializer.Serialize(objectToSerialize, options);
    }

    public T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _defaultJsonDeserializerOptions);
    }

    /// <summary>
    /// Creates the default Json serializer options for System.Text.Json parsing.
    /// </summary>
    private JsonSerializerOptions CreateDefaultJsonDeserializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            AllowTrailingCommas = true,
            RespectNullableAnnotations = false,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    static typeInfo =>
                    {
                        if (typeInfo.Kind != JsonTypeInfoKind.Object) {
                            return;
                        }

                        foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
                        {
                            // Strip IsRequired constraint from every property.
                            propertyInfo.IsRequired = false;
                        }
                    }
                }
            }
        };

        // Add date converter with the desired format
        //options.Converters.Add(new JsonDateConverter("yyyy-MM-dd"));
        options.Converters.Add(new JsonStringEnumConverter());

        // Add your custom converters adapted for System.Text.Json here:
        options.Converters.Add(new PolymorphicConverter<PaymentResponse>(new PaymentResponseFactory(), "method"));
        options.Converters.Add(new PolymorphicConverter<MandateResponse>(new MandateResponseFactory(), "method"));
        options.Converters.Add(new PolymorphicConverter<BalanceReportResponse>(new BalanceReportResponseFactory(), "grouping"));
        options.Converters.Add(new PolymorphicConverter<BalanceTransactionResponse>(new BalanceTransactionFactory(), "type"));

        return options;
    }
}

/// <summary>
/// Custom converter to handle date format yyyy-MM-dd in System.Text.Json.
/// </summary>
public class JsonDateConverter : JsonConverter<DateTime>
{
    private readonly string _format;

    public JsonDateConverter(string format)
    {
        _format = format;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null)
            throw new JsonException("Expected date string value.");

        return DateTime.ParseExact(value, _format, null);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}
