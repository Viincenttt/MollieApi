using System.Net;
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
        var result = await _paymentClient.GetPaymentAsync(id);
        if (result.Success) {
            _logger.LogInformation("Webhook called for PaymentId={PaymentId}, PaymentStatus={Status}",
                id,
                result.Data.Status);

            return Ok();
        }

        if (result.HttpStatusCode == HttpStatusCode.NotFound) {
            // The webhook URL is public, so anyone can call it with a made-up id. Respond with 200 OK, so the
            // caller does not learn whether the id exists and Mollie does not keep retrying an unknown payment
            _logger.LogWarning("Webhook called for unknown PaymentId={PaymentId}", id);
            return Ok();
        }

        // Any other failure is likely temporary. Respond with an error status code without details, so Mollie
        // calls the webhook again later
        _logger.LogError("Failed to retrieve payment for PaymentId={PaymentId}. Error: {Error}",
            id,
            result.Error);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
