using Shouldly;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Response;
using Xunit;

namespace Mollie.Tests.Unit.Models {
    public class AmountTests {
        [Theory]
        [InlineData("EUR", 50.25, "50.25")]
        [InlineData("EUR", 50, "50.00")]
        [InlineData("JPY", 51, "51")]
        [InlineData("ISK", 52, "52")]
        [InlineData("ISK", 52.40, "52")]
        public void CreateAmount_DecimalIsConverted_ValueHasCorrectFormat(string currency, decimal value, string expectedResult) {
            // Arrange & act
            var amount = new Amount(currency, value);

            // Assert
            amount.Value.ShouldBe(expectedResult);
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
    }
}
