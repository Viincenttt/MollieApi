using Microsoft.AspNetCore.Http;

namespace Mollie.Api.AspNet.Webhooks.Authorization;

#if NET7_0_OR_GREATER

public class MollieSignatureEndpointFilter : IEndpointFilter {
    private readonly MollieSignatureValidator _signatureValidator;

    public MollieSignatureEndpointFilter(MollieSignatureValidator signatureValidator) {
        _signatureValidator = signatureValidator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        var request = context.HttpContext.Request;
        bool isValid = await _signatureValidator.Validate(request);
        if (!isValid) {
            return Results.Unauthorized();
        }

        return await next(context);
    }
}
#endif
