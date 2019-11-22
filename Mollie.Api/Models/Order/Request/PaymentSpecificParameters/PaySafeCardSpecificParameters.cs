﻿namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public class PaySafeCardSpecificParameters : PaymentSpecificParameters {
        /// <summary>
        /// Used for consumer identification. For example, you could use the consumer’s IP address.
        /// </summary>
        public string CustomerReference { get; set; }
    }
}
