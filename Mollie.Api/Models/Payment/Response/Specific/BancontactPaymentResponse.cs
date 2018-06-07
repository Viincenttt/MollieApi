namespace Mollie.Api.Models.Payment.Response {
    public class BancontactPaymentResponse : PaymentResponse {
        public BancontactPaymentResponseDetails Details { get; set; }
    }

    public class BancontactPaymentResponseDetails {
        public string CardNumber { get; set; }
    }
}