using Mollie.Api.AspNet.Webhooks.Authorization;
using Mollie.Api.AspNet.Webhooks.ModelBinding;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.WebhookEvent.Response;

namespace Mollie.WebApplication.Blazor.Endpoints;

public static class WebhookHandler {

    public static void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app
            .MapPost(
                "api/webhook/minimalapi/example",
                async (MollieModelBinder<FullWebhookEventResponse<PaymentLinkResponse>> data, CancellationToken cancellationToken) =>
                {
                    return Results.Ok();
                })
            .AddEndpointFilter<MollieSignatureEndpointFilter>();
    }
}
