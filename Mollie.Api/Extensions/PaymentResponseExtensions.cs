using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Extensions
{
    public static class PaymentResponseExtensions
    {
        public static bool IsRefunded(this PaymentResponse paymentResponse)
        {
            var amountRefunded = paymentResponse?.AmountRefunded?.Value;
            return !string.IsNullOrEmpty(amountRefunded) && amountRefunded != "0.00";
        }

        public static bool IsChargeback(this PaymentResponse paymentResponse)
        {
            var chargebacks = paymentResponse?.Links?.Chargebacks;
            return chargebacks != null;
        }

        public static PaymentStatus StatusExtended(this PaymentResponse paymentResponse)
        {
            if (paymentResponse.Status == PaymentStatus.Paid)
            {
                if (paymentResponse.IsRefunded())
                {
                    return PaymentStatus.Refunded;
                }
                if (paymentResponse.IsChargeback())
                {
                    return PaymentStatus.Charged_Back;
                }
            }
            return paymentResponse.Status ?? PaymentStatus.Open;
        }
    }
}