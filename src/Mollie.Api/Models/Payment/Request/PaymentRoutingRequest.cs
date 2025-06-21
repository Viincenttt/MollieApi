using System;
using System.Text.Json.Serialization;
using Mollie.Api.Framework;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Payment.Request
{
    public record PaymentRoutingRequest
    {
        /// <summary>
        /// If more than one routing object is given, the routing objects must indicate what portion of the total payment amount is being routed.
        /// </summary>
        public Amount? Amount { get; set; }

        /// <summary>
        /// The destination of this portion of the payment.
        /// </summary>
        public required RoutingDestination Destination { get; set; }

        /// <summary>
        /// Optionally, schedule this portion of the payment to be transferred to its destination on a later date. If no date is given, the funds become available to the balance as soon as the payment succeeds.
        /// </summary>
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime? ReleaseDate { get; set; }
    }
}
