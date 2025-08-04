using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Webhook;

public class CreateWebhookModel {
    [Required]
    public required string Name { get; set; }

    [Required]
    [Url]
    public required string Url { get; set; }

    [Required]
    public required string EventTypes { get; set; } // TODO: Add a list of valid event types

    public bool Testmode { get; set; } = true;
}
