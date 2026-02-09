using System;
using System.Collections.Generic;
using System.Text;
using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Session; 
public record SessionLine : PaymentLine {

    /// <summary>
    /// The details of subsequent recurring billing cycles. These parameters are used in the Mollie Checkout to inform the shopper of the details for recurring products in the payments.
    /// </summary>
    public SessionLineRecurringDetails? Recurring { get; set; }
}
