using System.Linq;
using System.Reflection;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework;
using Xunit;

namespace Mollie.Tests.Unit;

public class DependencyInjectionTests
{
    [Fact]
    public void AddMollieClient_ShouldRegisterAllApiInterfaces()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMollieApi(options =>
        {
            options.ApiKey = "access_api-key";
            options.ClientId = "client-id";
            options.ClientSecret = "client-secret";
            options.RetryPolicy = MollieHttpRetryPolicies.TransientHttpErrorRetryPolicy();
        });

        // Act
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert
        var assembly = Assembly.GetAssembly(typeof(IPaymentClient));
        var apiClientInterfaces = assembly
            .GetTypes()
            .Where(type => type.Namespace == typeof(IPaymentClient).Namespace);
        foreach (var apiClientInterface in apiClientInterfaces)
        {
            if (apiClientInterface != typeof(IBaseMollieClient))
            {
                var apiClientImplementation = serviceProvider.GetService(apiClientInterface);
                apiClientImplementation.ShouldNotBeNull();
            }
        }
    }
}
