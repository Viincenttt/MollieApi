using System.Text.Json;
using Mollie.Api.JsonConverters;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Framework;

public class DecimalToStringConverterTests {
    public static TheoryData<decimal, string> WriteCases => new() {
        { 21.00m, "21.00" },
        { 6.012345m, "6.012345" },
        { 21m, "21" },
        { 0m, "0" }
    };

    [Theory]
    [MemberData(nameof(WriteCases))]
    public void Write_SerializesDecimalAsStringPreservingExactValue(decimal value, string expected) {
        // Given
        var converter = new DecimalToStringConverter();

        // When
        string json = JsonSerializer.Serialize(value, new JsonSerializerOptions {
            Converters = { converter }
        });

        // Then
        json.ShouldBe($"\"{expected}\"");
    }

    public static TheoryData<string, decimal> ReadCases => new() {
        { "\"21.00\"", 21.00m },
        { "\"6.012345\"", 6.012345m },
        { "\"21\"", 21m }
    };

    [Theory]
    [MemberData(nameof(ReadCases))]
    public void Read_DeserializesStringToDecimal(string json, decimal expected) {
        // Given
        var converter = new DecimalToStringConverter();

        // When
        decimal result = JsonSerializer.Deserialize<decimal>(json, new JsonSerializerOptions {
            Converters = { converter }
        });

        // Then
        result.ShouldBe(expected);
    }

    [Fact]
    public void Read_NullValue_ThrowsJsonException() {
        // Given
        var converter = new DecimalToStringConverter();

        // When / Then
        Should.Throw<JsonException>(() =>
            JsonSerializer.Deserialize<decimal>("null", new JsonSerializerOptions {
                Converters = { converter }
            }));
    }

    [Fact]
    public void NullableConverter_Write_NullValue_WritesNull() {
        // Given
        var converter = new NullableDecimalToStringConverter();

        // When
        string json = JsonSerializer.Serialize<decimal?>(null, new JsonSerializerOptions {
            Converters = { converter }
        });

        // Then
        json.ShouldBe("null");
    }

    [Fact]
    public void NullableConverter_Write_Value_SerializesAsString() {
        // Given
        var converter = new NullableDecimalToStringConverter();

        // When
        string json = JsonSerializer.Serialize<decimal?>(21.00m, new JsonSerializerOptions {
            Converters = { converter }
        });

        // Then
        json.ShouldBe("\"21.00\"");
    }

    [Fact]
    public void NullableConverter_Read_NullValue_ReturnsNull() {
        // Given
        var converter = new NullableDecimalToStringConverter();

        // When
        decimal? result = JsonSerializer.Deserialize<decimal?>("null", new JsonSerializerOptions {
            Converters = { converter }
        });

        // Then
        result.ShouldBeNull();
    }
}
