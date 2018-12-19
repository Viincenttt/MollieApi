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
    }
}