using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Mollie.Api.Extensions;
using Xunit;

namespace Mollie.Tests.Unit.Extensions
{
    public class DictionaryExtensionsTests
    {
        [Fact]
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
            result.Should().Be(expected);
        }

        [Fact]
        public void ToQueryString_WhenDictionaryIsEmpty_QueryStringIsEmpty()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();
            string expected = string.Empty;

            // Act
            var result = parameters.ToQueryString();

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void AddValueIfNotNullOrEmpty_ValueIsNotNull_ValueIsAdded()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();
            var parameterName = "include";
            var parameterValue = "issuers";

            // Act
            parameters.AddValueIfNotNullOrEmpty(parameterName, parameterValue);

            // Assert
            parameters.Should().NotBeEmpty();
            parameters[parameterName].Should().Be(parameterValue);
        }

        [Fact]
        public void AddValueIfNotNullOrEmpty_ValueIsEmpty_ValueIsNotAdded()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();

            // Act
            parameters.AddValueIfNotNullOrEmpty("include", "");

            // Assert
            parameters.Should().BeEmpty();
        }
    }
}
