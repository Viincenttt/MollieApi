using Mollie.Api.Models;
using NUnit.Framework;

namespace Mollie.Tests.Unit.Models {
    public class AmountTests {
        [TestCase("EUR", 50.25, "50.25")]
        [TestCase("EUR", 50, "50.00")]
        [TestCase("JPY", 51, "51")]
        [TestCase("ISK", 52, "52")]
        [TestCase("ISK", 52.40, "52")]
        public void CreateAmount_DecimalIsConverted_ValueHasCorrectFormat(string currency, decimal value, string expectedResult) {
            // Arrange & act
            Amount amount = new Amount(currency, value);
            
            // Assert
            Assert.AreEqual(expectedResult, amount.Value);
        }
    }
}