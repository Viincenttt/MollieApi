using System;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Payment.Response.PaymentSpecificParameters;

namespace Mollie.Api.Framework.Factories {
    internal class PaymentResponseFactory {
        public PaymentResponse Create(string? paymentMethod) {
            if (string.IsNullOrEmpty(paymentMethod)) {
                return Activator.CreateInstance<PaymentResponse>();
            }
            
            switch (paymentMethod) {
                case PaymentMethod.BankTransfer:
                    return Activator.CreateInstance<BankTransferPaymentResponse>();
                case PaymentMethod.CreditCard:
                    return Activator.CreateInstance<CreditCardPaymentResponse>();
                case PaymentMethod.Ideal:
                    return Activator.CreateInstance<IdealPaymentResponse>();
                case PaymentMethod.Giropay:
                    return Activator.CreateInstance<GiropayPaymentResponse>();
                case PaymentMethod.Eps:
                    return Activator.CreateInstance<EpsPaymentResponse>();
                case PaymentMethod.Bancontact:
                    return Activator.CreateInstance<BancontactPaymentResponse>();
                case PaymentMethod.PayPal:
                    return Activator.CreateInstance<PayPalPaymentResponse>();
                case PaymentMethod.PaySafeCard:
                    return Activator.CreateInstance<PaySafeCardPaymentResponse>();
                case PaymentMethod.Sofort:
                    return Activator.CreateInstance<SofortPaymentResponse>();
                case PaymentMethod.Belfius:
                    return Activator.CreateInstance<BelfiusPaymentResponse>();
                case PaymentMethod.DirectDebit:
                    return Activator.CreateInstance<SepaDirectDebitResponse>();
                case PaymentMethod.Kbc:
                    return Activator.CreateInstance<KbcPaymentResponse>();
                case PaymentMethod.GiftCard:
                    return Activator.CreateInstance<GiftcardPaymentResponse>();
                case PaymentMethod.IngHomePay:
                    return Activator.CreateInstance<IngHomePayPaymentResponse>();
                case PaymentMethod.PointOfSale:
                    return Activator.CreateInstance<PointOfSalePaymentResponse>();
                default:
                    return Activator.CreateInstance<PaymentResponse>();
            }
        }
    }
}