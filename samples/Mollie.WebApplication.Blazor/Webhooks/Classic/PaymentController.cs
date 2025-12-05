using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.WebApplication.Blazor.Webhooks.Classic;

[ApiController]
[Route("api/webhook/classic/controllers")]
public class PaymentController : ControllerBase {
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentClient _paymentClient;

    public PaymentController(ILogger<PaymentController> logger, IPaymentClient paymentClient) {
        _logger = logger;
        _paymentClient = paymentClient;
    }

    [HttpPost]
    public async Task<ActionResult> Webhook([FromForm] string id) {
        PaymentResponse payment = await _paymentClient.GetPaymentAsync(id);
        _logger.LogInformation("Webhook called for PaymentId={PaymentId}, PaymentStatus={Status}",
            id,
            payment.Status);

        return Ok();
    }
}
