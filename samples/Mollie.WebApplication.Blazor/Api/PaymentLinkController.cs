using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mollie.Api.AspNet.Webhooks.Nextgen;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.WebhookEvent;
using Mollie.Api.Models.WebhookEvent.Response;

namespace Mollie.WebApplication.Blazor.Api;

[ApiController]
[Route("api/paymentlink")]
public class PaymentLinkController : ControllerBase {
    private readonly ILogger<PaymentLinkController> _logger;
    private readonly IPaymentLinkClient _paymentLinkClient;

    public PaymentLinkController(ILogger<PaymentLinkController> logger, IPaymentLinkClient paymentLinkClient) {
        _logger = logger;
        _paymentLinkClient = paymentLinkClient;
    }

    [HttpPost]
    public async Task<ActionResult> Webhook([FromWebhookJson] FullWebhookEventResponse<PaymentLinkResponse> data) {
        return Ok();
    }
}
