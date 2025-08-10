using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mollie.Api.Framework;

namespace Mollie.Api.AspNet.Webhooks.Nextgen;

public class WebhookJsonModelBinder : IModelBinder {
    private readonly JsonConverterService _jsonConverterService;

    public WebhookJsonModelBinder() {
        _jsonConverterService = new();
    }

    public async Task BindModelAsync(ModelBindingContext context)
    {
        var request = context.HttpContext.Request;
        request.EnableBuffering();

        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        try {
            var result = _jsonConverterService.Deserialize(body, context.ModelType);
            context.Result = ModelBindingResult.Success(result);
        }
        catch (Exception ex)
        {
            context.ModelState.AddModelError(context.ModelName, ex.Message);
        }
    }
}

