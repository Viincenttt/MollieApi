using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mollie.Api.AspNet.Webhooks.Authorization;

public class MollieSignatureFilter : IAsyncAuthorizationFilter
{
    private readonly MollieSignatureValidator _signatureValidator;

    public MollieSignatureFilter(MollieSignatureValidator signatureValidator) {
        _signatureValidator = signatureValidator;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        bool isValid = await _signatureValidator.Validate(request);
        if (!isValid) {
            context.Result = new UnauthorizedResult();
        }
    }
}
