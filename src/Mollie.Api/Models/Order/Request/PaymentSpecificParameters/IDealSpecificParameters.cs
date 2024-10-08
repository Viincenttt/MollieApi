﻿namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record IDealSpecificParameters : OrderPaymentParameters {
        /// <summary>
        /// Optional - iDEAL issuer id. The id could for example be ideal_INGBNL2A. The returned paymentUrl will then directly
        /// point to the ING web site.
        /// </summary>
        public string? Issuer { get; set; }
    }
}
