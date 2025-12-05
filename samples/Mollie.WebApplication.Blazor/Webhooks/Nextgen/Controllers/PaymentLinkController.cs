using Microsoft.AspNetCore.Mvc;
using Mollie.Api.AspNet.Webhooks.Authorization;
using Mollie.Api.AspNet.Webhooks.ModelBinding;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.WebhookEvent.Response;

namespace Mollie.WebApplication.Blazor.Webhooks.Nextgen.Controllers;

[ApiController]
[Route("api/webhook/nextgen/controllers")]
public class PaymentLinkController : ControllerBase {
    // Example of full webhook event with specific data entity type, such as PaymentLinkResponse
    [HttpPost("full/specific")]
    [ServiceFilter(typeof(MollieSignatureFilter))]
    public Task<ActionResult> WebhookWithSpecificType([FromMollieWebhook] FullWebhookEventResponse<PaymentLinkResponse> data) {
        return Task.FromResult<ActionResult>(Ok());
    }

    // Example of full webhook event with generic data entity type
    [HttpPost("full/generic")]
    [ServiceFilter(typeof(MollieSignatureFilter))]
    public Task<ActionResult> WebhookWithGenericType([FromMollieWebhook] FullWebhookEventResponse data) {
        return Task.FromResult<ActionResult>(Ok());
    }

    // Example of a simple webhook event response, that does not include the entity data
    [HttpPost("simple")]
    [ServiceFilter(typeof(MollieSignatureFilter))]
    public Task<ActionResult> WebhookWithGenericType([FromMollieWebhook] SimpleWebhookEventResponse data) {
        return Task.FromResult<ActionResult>(Ok());
    }
}
