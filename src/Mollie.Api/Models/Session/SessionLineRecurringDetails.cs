using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Session; 
public record SessionLineRecurringDetails {
    /// <summary>
    /// A description of the recurring item. If not present, the main description of the item will be used.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Cadence unit of the recurring item. For example: 12 months, 52 weeks or 365 days.
    /// </summary>
    public required string Interval { get; set; }

    /// <summary>
    /// Total amount and currency of the recurring item.
    /// </summary>
    public required Amount Amount { get; set; }

    /// <summary>
    /// Optional – Total number of charges for the subscription to complete. Leave empty for ongoing subscription.
    /// </summary>
    public int? Times { get; set; }

    /// <summary>
    /// Optional – The start date of the subscription if it does not start right away (format YYYY-MM-DD)
    /// </summary>
    [JsonConverter(typeof(DateJsonConverter))]
    public DateTime? StartDate { get; set; }
}
