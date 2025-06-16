using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Newtonsoft.Json;

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
    public async Task<ActionResult> Webhook([FromBody] WebhookEvent webhookData) {
        return Ok();
    }
}

public class WebhookEvent {
    public required string Resource { get; set; }
    public required string Id { get; set; }
    public required string Type { get; set; }
    public required string EntityId { get; set; }

    [JsonProperty("_embedded")]
    public object? Embedded { get; set; }
}
