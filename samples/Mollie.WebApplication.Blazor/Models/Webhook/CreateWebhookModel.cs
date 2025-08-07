using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Webhook;

public class CreateWebhookModel {
    [Required]
    public required string Name { get; set; }

    [Required]
    [Url]
    public required string Url { get; set; }

    public required List<string> EventTypes { get; set; } = new();

    public bool Testmode { get; set; } = true;
}
