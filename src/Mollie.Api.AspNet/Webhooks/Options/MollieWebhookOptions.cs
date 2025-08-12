namespace Mollie.Api.AspNet.Webhooks.Options;

public record MollieWebhookOptions {
    public string Secret { get; set; } = string.Empty;
}
