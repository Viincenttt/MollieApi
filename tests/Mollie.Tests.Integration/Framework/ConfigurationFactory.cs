using Microsoft.Extensions.Configuration;
using System.IO;

namespace Mollie.Tests.Integration.Framework {
    public static class ConfigurationFactory {
        public static IConfiguration GetConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(typeof(ConfigurationFactory).Assembly)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return configuration;
        }
    }
}
