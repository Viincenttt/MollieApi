using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        using var reader = new StreamReader(Request.Body);
        _logger.LogInformation("Content-Type: {ContentType}", Request.ContentType);
        var content = await reader.ReadToEndAsync();

        return Ok();
    }
}

public class FromWebhookJsonAttribute : ModelBinderAttribute
{
    public FromWebhookJsonAttribute() : base(typeof(WebhookJsonModelBinder)) { }
}

public class WebhookJsonModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext context)
    {
        var request = context.HttpContext.Request;
        request.EnableBuffering();

        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            AllowTrailingCommas = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    static typeInfo =>
                    {
                        if (typeInfo.Kind != JsonTypeInfoKind.Object) {
                            return;
                        }

                        foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
                        {
                            // Strip IsRequired constraint from every property.
                            propertyInfo.IsRequired = false;
                        }
                    }
                }
            }
        };

        options.Converters.Add(new JsonStringEnumConverter());
        // TODO: Add this to generic package so we can register these converters
        /*
        options.Converters.Add(new PolymorphicConverter<PaymentResponse>(new PaymentResponseFactory(), "method"));
        options.Converters.Add(new PolymorphicConverter<MandateResponse>(new MandateResponseFactory(), "method"));
        options.Converters.Add(new PolymorphicConverter<BalanceReportResponse>(new BalanceReportResponseFactory(), "grouping"));
        options.Converters.Add(new PolymorphicConverter<BalanceTransactionResponse>(new BalanceTransactionFactory(), "type"));*/

        try
        {
            var result = JsonSerializer.Deserialize(body, context.ModelType, options);
            context.Result = ModelBindingResult.Success(result);
        }
        catch (Exception ex)
        {
            context.ModelState.AddModelError(context.ModelName, ex.Message);
        }
    }
}
