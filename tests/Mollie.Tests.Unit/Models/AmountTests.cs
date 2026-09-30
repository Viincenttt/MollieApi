using System.Globalization;
using System.Text.Json;
using Mollie.Api.Framework;
using Shouldly;
using Mollie.Api.Models;
using Xunit;

namespace Mollie.Tests.Unit.Models {
    public class AmountTests {
        [Theory]
        [InlineData("EUR", 50.25)]
        [InlineData("EUR", 50)]
        [InlineData("JPY", 51)]
        [InlineData("ISK", 52)]
        [InlineData("ISK", 52.40)]
        public void CreateAmount_DecimalValue_ValueIsSet(string currency, decimal value) {
            // Arrange & act
            var amount = new Amount(currency, value);

            // Assert
            amount.Value.ShouldBe(value);
        }

        [Fact]
        public void Amount_ConvertedToDecimal_IsEqualToOriginalValue() {
            // Arrange
            decimal originalValue = 50.25m;
            var amount = new Amount(Currency.EUR, originalValue);

            // Act
            decimal convertedValue = amount;

            // Assert
            convertedValue.ShouldBe(originalValue);
        }

        [Fact]
        public void NullableAmount_ConvertedToNullableDecimal_IsEqualToOriginalValue() {
            // Arrange
            decimal originalValue = 50.25m;
            Amount? amount = new(Currency.EUR, originalValue);

            // Act
            decimal? convertedValue = amount;

            // Assert
            convertedValue.ShouldBe(originalValue);
        }

        [Fact]
        public void NullAmount_ConvertedToNullableDecimal_IsNull() {
            // Arrange
            Amount? amount = null;

            // Act
            decimal? convertedValue = amount;

            // Assert
            convertedValue.ShouldBeNull();
        }

        [Theory]
        [InlineData("EUR", "20", "20.00")]
        [InlineData("EUR", "20.0", "20.00")]
        [InlineData("EUR", "20.00", "20.00")]
        [InlineData("EUR", "86.1000", "86.10")]
        [InlineData("EUR", "-5.5", "-5.50")]
        [InlineData("EUR", "0", "0.00")]
        [InlineData("eur", "20", "20.00")]
        [InlineData("USD", "1234567.8", "1234567.80")]
        [InlineData("JPY", "500", "500")]
        [InlineData("JPY", "500.00", "500")]
        [InlineData("jpy", "500.00", "500")]
        [InlineData("ISK", "52.0", "52")]
        [InlineData("XXX", "20", "20.00")]
        public void Amount_Serialized_ValueHasNumberOfDecimalsOfCurrency(string currency, string value, string expectedValue) {
            // Arrange
            var amount = new Amount(currency, decimal.Parse(value, CultureInfo.InvariantCulture));

            // Act
            string json = new JsonConverterService().Serialize(amount);

            // Assert
            json.ShouldBe($"{{\"currency\":\"{currency}\",\"value\":\"{expectedValue}\"}}");
        }

        [Theory]
        [InlineData("EUR", "10.005", "10.005")]
        [InlineData("EUR", "10.0050", "10.0050")]
        [InlineData("JPY", "500.5", "500.5")]
        [InlineData("ISK", "52.40", "52.40")]
        public void Amount_Serialized_ValueWithTooManyDecimalsIsNotRounded(string currency, string value, string expectedValue) {
            // Arrange
            var amount = new Amount(currency, decimal.Parse(value, CultureInfo.InvariantCulture));

            // Act
            string json = new JsonConverterService().Serialize(amount);

            // Assert
            json.ShouldBe($"{{\"currency\":\"{currency}\",\"value\":\"{expectedValue}\"}}");
        }

        [Fact]
        public void Amount_SerializedWithDefaultSerializerOptions_ValueHasNumberOfDecimalsOfCurrency() {
            // Arrange
            var amount = new Amount(Currency.EUR, 20m);

            // Act
            string json = JsonSerializer.Serialize(amount);

            // Assert
            json.ShouldBe("{\"currency\":\"EUR\",\"value\":\"20.00\"}");
        }

        [Fact]
        public void Amount_SerializedAsPropertyOfRequest_ValueHasNumberOfDecimalsOfCurrency() {
            // Arrange
            var request = new { Amount = new Amount(Currency.EUR, 20m), Missing = (Amount?)null };

            // Act
            string json = new JsonConverterService().Serialize(request);

            // Assert
            json.ShouldBe("{\"amount\":{\"currency\":\"EUR\",\"value\":\"20.00\"}}");
        }

        [Theory]
        [InlineData("{\"currency\":\"EUR\",\"value\":\"20\"}")]
        [InlineData("{\"value\":20.00,\"currency\":\"EUR\"}")]
        [InlineData("{\"currency\":\"EUR\",\"unknown\":{\"nested\":[1,2]},\"value\":\"20.00\"}")]
        public void Amount_Deserialized_ValueIsParsedToDecimal(string json) {
            // Arrange
            var jsonConverterService = new JsonConverterService();

            // Act
            Amount? amount = jsonConverterService.Deserialize<Amount>(json);

            // Assert
            amount.ShouldNotBeNull();
            amount.Currency.ShouldBe(Currency.EUR);
            amount.Value.ShouldBe(20.00m);
        }

        [Theory]
        [InlineData("{\"currency\":\"EUR\"}")]
        [InlineData("{\"value\":\"20.00\"}")]
        [InlineData("{\"currency\":\"EUR\",\"value\":\"abc\"}")]
        [InlineData("{\"currency\":\"EUR\",\"value\":null}")]
        public void Amount_DeserializedFromInvalidJson_ThrowsJsonException(string json) {
            // Arrange
            var jsonConverterService = new JsonConverterService();

            // Act & assert
            Should.Throw<JsonException>(() => jsonConverterService.Deserialize<Amount>(json));
        }

        [Fact]
        public void NullAmount_Deserialized_IsNull() {
            // Arrange
            var jsonConverterService = new JsonConverterService();

            // Act
            Amount? amount = jsonConverterService.Deserialize<Amount>("null");

            // Assert
            amount.ShouldBeNull();
        }

        [Fact]
        public void Amount_Deserialized_StringValueIsParsedToDecimal() {
            // Arrange
            const string json = "{\"currency\":\"EUR\",\"value\":\"20.00\"}";
            var jsonConverterService = new JsonConverterService();

            // Act
            Amount? amount = jsonConverterService.Deserialize<Amount>(json);

            // Assert
            amount.ShouldNotBeNull();
            amount.Currency.ShouldBe(Currency.EUR);
            amount.Value.ShouldBe(20.00m);
        }
    }
}
