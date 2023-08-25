using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Payment.Response.Specific;

namespace Mollie.Api.Framework.Factories {
    public class PaymentResponseFactory {
        public PaymentResponse Create(string paymentMethod) {
            switch (paymentMethod) {
                case PaymentMethod.BankTransfer:
                    return new BankTransferPaymentResponse();
                case PaymentMethod.CreditCard:
                    return new CreditCardPaymentResponse();
                case PaymentMethod.Ideal:
                    return new IdealPaymentResponse();
                case PaymentMethod.Giropay:
                    return new GiropayPaymentResponse();
                case PaymentMethod.Eps:
                    return new EpsPaymentResponse();
                case PaymentMethod.Bancontact:
                    return new BancontactPaymentResponse();
                case PaymentMethod.PayPal:
                    return new PayPalPaymentResponse();
                case PaymentMethod.PaySafeCard:
                    return new PaySafeCardPaymentResponse();
                case PaymentMethod.Sofort:
                    return new SofortPaymentResponse();
                case PaymentMethod.Belfius:
                    return new BelfiusPaymentResponse();
                case PaymentMethod.DirectDebit:
                    return new SepaDirectDebitResponse();
                case PaymentMethod.Kbc:
                    return new KbcPaymentResponse();
                case PaymentMethod.GiftCard:
                    return new GiftcardPaymentResponse();
                case PaymentMethod.IngHomePay:
                    return new IngHomePayPaymentResponse();
                case PaymentMethod.PointOfSale:
                    return new PointOfSalePaymentResponse();
                default:
                    return new PaymentResponse();
            }
        }
    }
}