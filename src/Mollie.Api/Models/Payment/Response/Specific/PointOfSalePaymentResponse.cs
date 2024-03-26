namespace Mollie.Api.Models.Payment.Response.Specific {
    public class PointOfSalePaymentResponse : PaymentResponse {
        public required PointOfSalePaymentResponseDetails Details { get; init; }
    }
    
    public class PointOfSalePaymentResponseDetails {
        public required string TerminalId { get; init; }
    }
}