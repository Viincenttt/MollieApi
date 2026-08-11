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

        public static TheoryData<decimal, string> SerializationCases => new() {
            { 20m, "\"20\"" },
            { 20.00m, "\"20.00\"" },
            { 86.1000m, "\"86.1000\"" }
        };

        [Theory]
        [MemberData(nameof(SerializationCases))]
        public void Amount_Serialized_ValueIsSerializedAsStringPreservingScale(decimal value, string expectedJson) {

            // Arrange
            var amount = new Amount(Currency.EUR, value);

            // Act
            string json = JsonSerializer.Serialize(amount);

            // Assert
            json.ShouldContain($"\"value\":{expectedJson}");
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
