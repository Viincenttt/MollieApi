namespace Mollie.Api.Models.Order.SpecificParameters
{
    public class BankTransferSpecificParameters : PaymentSpecificParameters
    {
        /// <summary>
        /// Optional - Consumer's e-mail address, to automatically send the bank transfer details to. Please note: the payment
        /// instructions will be sent immediately when creating the payment. if you don't specify the locale parameter, the
        /// email will be sent in English, as we haven't yet been able to detect the consumer's browser language.
        /// </summary>
        public string BillingEmail { get; set; }

        /// <summary>
        /// Optional - The date the payment should expire, in YYYY-MM-DD format. Please note: The minimum date is tomorrow and
        /// the maximum date is 100 days.
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Optional - The locale will determine the target bank account the customer has to transfer the money to.
        /// </summary>
        public string Locale { get; set; }
    }
}
