using System;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Session;

public record SessionPaymentDetails {

    /// <summary>
    /// The webhook URL where we will send payment status updates to.
    /// This URL will be automatically set as the webhook URL for all payments created for this session.
    /// </summary>
    public string? WebhookUrl { get; set; }
}
