using System;

namespace Mollie.Api.Models.Payment.Request
{
    public class PaymentRoutingRequest
    {
        /// <summary>
        /// If more than one routing object is given, the routing objects must indicate what portion of the total payment amount is being routed.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// The destination of this portion of the payment.
        /// </summary>
        public RoutingDestination Destination { get; set; }

        /// <summary>
        /// Optionally, schedule this portion of the payment to be transferred to its destination on a later date. If no date is given, the funds become available to the balance as soon as the payment succeeds.
        /// </summary>
        public DateTime? ReleaseDate { get; set; }
    }
}
