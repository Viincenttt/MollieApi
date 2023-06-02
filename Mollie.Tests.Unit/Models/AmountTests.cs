using FluentAssertions;
using Mollie.Api.Models;
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
            Amount amount = new Amount(currency, value);
            
            // Assert
            amount.Value.Should().Be(expectedResult);
        }
    }
}