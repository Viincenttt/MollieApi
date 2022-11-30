using Mollie.Api.Models;
using NUnit.Framework;
using System;

namespace Mollie.Tests.Unit.Framework {

    [TestFixture]
    public class AmountConversionTests {
        [Test]
        public void InvalidAmountValueWillThrowInvalidCastException() {

            // Initiate Amount with invalid decimal value
            var amount = new Amount() { Currency = Currency.EUR, Value = "NotAValidDecimal" };

            // When: We implicitly cast Amount to decimal
            // Then: An InvalidCastException will be thrown
            Assert.Throws<InvalidCastException>(() => {
                decimal a = amount;
            });
        }
    }
}
