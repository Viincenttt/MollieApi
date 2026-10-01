namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record IdealPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with the consumer bank account details.
        /// </summary>
        public required IdealPaymentResponseDetails? Details { get; set; }
    }

    public record IdealPaymentResponseDetails : PaymentResponseDetails {
        /// <summary>
        /// Include a QR code object. Only available for iDEAL, Bancontact and bank transfer payments.
        /// </summary>
        public QrCode? QrCode { get; set; }
    }
}
