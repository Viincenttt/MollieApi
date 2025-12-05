using Microsoft.AspNetCore.Mvc;

namespace Mollie.Api.AspNet.Webhooks.ModelBinding;

public class FromMollieWebhookAttribute : ModelBinderAttribute
{
    public FromMollieWebhookAttribute() : base(typeof(FromMollieWebhookModelBinder)) { }
}
