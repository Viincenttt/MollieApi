using Mollie.Api.AspNet.Webhooks.Authorization;
using Mollie.Api.AspNet.Webhooks.ModelBinding;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.WebhookEvent.Response;

namespace Mollie.WebApplication.Blazor.Webhooks.Nextgen.MinimalApi;

public static class WebhookHandler {

    public static void RegisterEndpoints(IEndpointRouteBuilder app) {
        // Example of full webhook event with specific data entity type, such as PaymentLinkResponse
        app
            .MapPost("api/webhook/nextgen/minimalapi/full/specific",
                (MollieModelBinder<FullWebhookEventResponse<PaymentLinkResponse>> data) => {
                    if (data.Model == null) {
                        return Results.BadRequest();
                    }

                    return Results.Ok();
                })
            .AddEndpointFilter<MollieSignatureEndpointFilter>();

        // Example of full webhook event with generic data entity type
        app
            .MapPost("api/webhook/nextgen/minimalapi/full/generic",
                (MollieModelBinder<FullWebhookEventResponse> data) => {
                    if (data.Model == null) {
                        return Results.BadRequest();
                    }

                    return Results.Ok();
                })
            .AddEndpointFilter<MollieSignatureEndpointFilter>();

        // Example of a simple webhook event response, that does not include the entity data
        app
            .MapPost("api/webhook/nextgen/minimalapi/simple",
                (MollieModelBinder<SimpleWebhookEventResponse> data) => Results.Ok())
            .AddEndpointFilter<MollieSignatureEndpointFilter>();
    }
}
