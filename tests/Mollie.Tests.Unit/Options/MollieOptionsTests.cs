using Mollie.Api.Options;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Options;

public class MollieOptionsTests
{
    [Fact]
    public void ToString_DoesNotContainSecrets()
    {
        // Arrange
        var options = new MollieOptions {
            ApiKey = "test_api-key-value",
            ClientId = "app_client-id",
            ClientSecret = "client-secret-value"
        };

        // Act
        string result = options.ToString();

        // Assert
        result.ShouldNotContain("test_api-key-value");
        result.ShouldNotContain("client-secret-value");
        result.ShouldContain("app_client-id");
    }
}
