using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.AspNet.Webhooks.Authorization;
using Mollie.Api.AspNet.Webhooks.Options;

namespace Mollie.Api.AspNet;

public static class DependencyInjection {
    public static IServiceCollection AddMollieWebhook(
        this IServiceCollection services,
        Action<MollieWebhookOptions> optionsDelegate) {

        MollieWebhookOptions options = new();
        optionsDelegate.Invoke(options);

        services.AddSingleton(options);

        services.AddScoped<MollieSignatureFilter>();
        services.AddSingleton<MollieSignatureValidator>();

        return services;
    }
}
