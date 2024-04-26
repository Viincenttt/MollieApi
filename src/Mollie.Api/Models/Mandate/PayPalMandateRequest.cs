﻿namespace Mollie.Api.Models.Mandate
{
    public record PayPalMandateRequest : MandateRequest
    {
        public PayPalMandateRequest() {
            Method = Payment.PaymentMethod.PayPal;
        }

        /// <summary>
        /// Required For Paypal - The consumer's email address.
        /// </summary>
        public required string ConsumerEmail { get; init; }

        /// <summary>
        /// Required for `paypal` mandates - The billing agreement ID given by PayPal.
        /// </summary>
        public required string PaypalBillingAgreementId { get; init; }
    }
}
