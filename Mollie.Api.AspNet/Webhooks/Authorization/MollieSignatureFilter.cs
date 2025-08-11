using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mollie.Api.AspNet.Webhooks.Options;

namespace Mollie.Api.AspNet.Webhooks.Authorization;

public class MollieSignatureFilter : IAsyncAuthorizationFilter
{
    private readonly MollieWebhookOptions _options;
    private readonly MollieSignatureValidator _signatureValidator;

    public MollieSignatureFilter(MollieWebhookOptions options, MollieSignatureValidator signatureValidator) {
        _options = options;
        _signatureValidator = signatureValidator;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;

        if (!request.Headers.TryGetValue("X-Mollie-Signature", out var headerValues))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var headerValue = headerValues.FirstOrDefault();
        if (string.IsNullOrEmpty(headerValue))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var bodyBytes = await GetRequestBodyBytes(request);

        var valid = _signatureValidator.Validate(headerValue, bodyBytes, _options.Secret);
        if (!valid)
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private async Task<byte[]> GetRequestBodyBytes(HttpRequest request)
    {
        request.EnableBuffering();

        request.Body.Seek(0, SeekOrigin.Begin);
        using var ms = new MemoryStream();
        await request.Body.CopyToAsync(ms);
        var bodyBytes = ms.ToArray();
        request.Body.Seek(0, SeekOrigin.Begin);

        return bodyBytes;
    }
}
