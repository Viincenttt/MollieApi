using Microsoft.AspNetCore.Mvc;

namespace Mollie.Api.AspNet.Webhooks.Nextgen;

public class FromWebhookJsonAttribute : ModelBinderAttribute
{
    public FromWebhookJsonAttribute() : base(typeof(WebhookJsonModelBinder)) { }
}
