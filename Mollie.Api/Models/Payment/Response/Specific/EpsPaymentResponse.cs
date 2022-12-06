namespace Mollie.Api.Models.Payment.Response
{
    public class EpsPaymentResponse : PaymentResponse
    {
        /// <summary>
        /// An object with the consumer bank account details.
        /// </summary>
        public EpsPaymentResponseDetails Details { get; set; }
    }

    public class EpsPaymentResponseDetails
    {
        /// <summary>
        /// Only available if the payment has been completed – The consumer's name.
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's IBAN.
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's bank's BIC.
        /// </summary>
        public string ConsumerBic { get; set; }
    }
}