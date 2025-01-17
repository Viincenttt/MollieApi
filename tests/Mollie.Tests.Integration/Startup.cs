using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mollie.Api;
using Mollie.Tests.Integration.Framework;

namespace Mollie.Tests.Integration;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder) => hostBuilder
        .ConfigureHostConfiguration(options => {
            options
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(typeof(ConfigurationFactory).Assembly)
                .AddEnvironmentVariables();
        })
        .ConfigureServices((context, services) => {
            services.AddMollieApi(options => {
                options.ApiKey = context.Configuration["Mollie:ApiKey"]!;
                options.RetryPolicy = MollieIntegrationTestHttpRetryPolicies.TooManyRequestRetryPolicy();
            });
        });
}
