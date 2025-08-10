using Microsoft.AspNetCore.Mvc;
using Mollie.Api.AspNet.Webhooks.ModelBinding;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.PaymentLink.Response;
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
    public async Task<ActionResult> Webhook([FromMollieWebhook] FullWebhookEventResponse<PaymentLinkResponse> data) {
        return Ok();
    }
}
