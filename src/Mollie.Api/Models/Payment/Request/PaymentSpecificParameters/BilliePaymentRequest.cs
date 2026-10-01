using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record BilliePaymentRequest : PaymentRequest {
        public BilliePaymentRequest() {
            Method = PaymentMethod.Billie;
        }

        [SetsRequiredMembers]
        public BilliePaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.Billie;
        }

        /// <summary>
        /// Billie is a business-to-business (B2B) payment method. It requires extra information to identify the
        /// organization that is completing the payment. It is recommended to include these parameters up front for a
        /// seamless flow. Otherwise, Billie will ask the customer to complete the missing fields during checkout.
        /// </summary>
        public PaymentCompanyDetails? Company { get; set; }
    }
}
