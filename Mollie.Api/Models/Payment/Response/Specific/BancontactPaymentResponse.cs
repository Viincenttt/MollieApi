namespace Mollie.Api.Models.Payment.Response.Specific {
    public class BancontactPaymentResponse : PaymentResponse {
        public BancontactPaymentResponseDetails Details { get; set; }
    }

    public class BancontactPaymentResponseDetails {
        /// <summary>
        /// Only available if the payment is completed - The last four digits of the card number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Only available if the payment is completed - Unique alphanumeric representation of card, usable for 
        /// identifying returning customers.
        /// </summary>
        public string CardFingerprint { get; set; }

        /// <summary>
        /// Only available if requested during payment creation - The QR code that can be scanned by the mobile 
        /// Bancontact application. This enables the desktop to mobile feature.
        /// </summary>
        public QrCode QrCode { get; set; }
    }
}