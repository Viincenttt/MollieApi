using Mollie.Api.Models;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Framework {
    public class AmountConversionTests {
        [Fact]
        public void Amount_ImplicitlyConvertedToDecimal_ReturnsValue() {
            // Given
            var amount = new Amount(Currency.EUR, 50.25m);

            // When
            decimal convertedValue = amount;

            // Then
            convertedValue.ShouldBe(50.25m);
        }
    }
}
