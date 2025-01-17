using System.Collections.Generic;
using Shouldly;
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
            result.ShouldBe(expected);
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
            result.ShouldBe(expected);
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
            parameters.ShouldNotBeEmpty();
            parameters[parameterName].ShouldBe(parameterValue);
        }

        [Fact]
        public void AddValueIfNotNullOrEmpty_ValueIsEmpty_ValueIsNotAdded()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();

            // Act
            parameters.AddValueIfNotNullOrEmpty("include", "");

            // Assert
            parameters.ShouldBeEmpty();
        }

        [Fact]
        public void AddValueIfTrue_ValueIsTrue_ValueIsAdded()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();
            var parameterName = "testmode";

            // Act
            parameters.AddValueIfTrue(parameterName, true);

            // Assert
            parameters.ShouldNotBeEmpty();
            parameters[parameterName].ShouldBe(bool.TrueString.ToLower());
        }

        [Fact]
        public void AddValueIfTrue_ValueIsFalse_ValueIsNotAdded()
        {
            // Arrange
            var parameters = new Dictionary<string, string>();

            // Act
            parameters.AddValueIfTrue("testmode", false);

            // Assert
            parameters.ShouldBeEmpty();
        }
    }
}
