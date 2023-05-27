namespace Mollie.Api.Models.Payment.Response.Specific {
    public class PointOfSalePaymentResponse : PaymentResponse {
        public PointOfSalePaymentResponseDetails Details { get; set; }
    }
    
    public class PointOfSalePaymentResponseDetails {
        public string TerminalId { get; set; }
    }
}