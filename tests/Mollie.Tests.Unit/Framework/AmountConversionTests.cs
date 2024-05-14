﻿using Mollie.Api.Models;
using System;
using Xunit;

namespace Mollie.Tests.Unit.Framework {
    public class AmountConversionTests {
        [Fact]
        public void InvalidAmountValueWillThrowInvalidCastException() {

            // Initiate Amount with invalid decimal value
            var amount = new Amount(Currency.EUR, "NotAValidDecimal");

            // When: We implicitly cast Amount to decimal
            // Then: An InvalidCastException will be thrown
            Assert.Throws<InvalidCastException>(() => {
                // ReSharper disable once UnusedVariable
                decimal a = amount;
            });
        }
    }
}
