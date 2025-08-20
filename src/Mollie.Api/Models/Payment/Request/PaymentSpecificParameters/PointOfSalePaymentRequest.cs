using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters;

public record PointOfSalePaymentRequest : PaymentRequest {
    public PointOfSalePaymentRequest() {
        Method = PaymentMethod.PointOfSale;
    }

    [SetsRequiredMembers]
    public PointOfSalePaymentRequest(PaymentRequest paymentRequest, string terminalId) : base(paymentRequest) {
        Method = PaymentMethod.PointOfSale;
        TerminalId = terminalId;
    }

    /// <summary>
    /// The ID of the terminal device where you want to initiate the payment on
    /// </summary>
    public required string TerminalId { get; set; }
}
