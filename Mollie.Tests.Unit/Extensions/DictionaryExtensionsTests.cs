using System.Collections.Generic;
using System.Linq;
using Mollie.Api.Extensions;
using NUnit.Framework;

namespace Mollie.Tests.Unit.Extensions
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [Test]
        public void ToQueryString_WhenMultipleKeyValuePairsAreAdded_MultipleParametersAreAddedToQueryString()
        {
            // Arrange
            var parameters = new Dictionary<string, string>()
            {
                {"include", "issuers"},
                {"testmode", "true"}
            };
            var expected = "?include=issuers&testmode=true";

            // Act
            var result = parameters.ToQueryString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToQueryString_WhenDictionaryIsEmpty_QueryStringIsEmpty()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();
            string expected = string.Empty;

            // Act
            var result = parameters.ToQueryString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AddValueIfNotNullOrEmpty_ValueIsNotNull_ValueIsAdded()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();
            var parameterName = "include";
            var parameterValue = "issuers";

            // Act
            parameters.AddValueIfNotNullOrEmpty(parameterName, parameterValue);

            // Assert
            Assert.IsTrue(parameters.Any());
            Assert.AreEqual(parameterValue, parameters[parameterName]);
        }

        [Test]
        public void AddValueIfNotNullOrEmpty_ValueIsEmpty_ValueIsNotAdded()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();

            // Act
            parameters.AddValueIfNotNullOrEmpty("include", "");

            // Assert
            Assert.IsFalse(parameters.Any());
        }
    }
}
