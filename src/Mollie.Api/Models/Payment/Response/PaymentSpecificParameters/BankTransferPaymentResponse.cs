using System.Runtime.Serialization;
using Mollie.Api.Models.Url;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record BankTransferPaymentResponse : PaymentResponse {
        public required BankTransferPaymentResponseDetails? Details { get; set; }

        /// <summary>
        /// For bank transfer payments, the _links object will contain some additional URL objects relevant to the payment.
        /// </summary>
        [JsonProperty("_links")]
        public new required BankTransferPaymentResponseLinks Links { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            base.Links = this.Links;
        }
    }

    public record BankTransferPaymentResponseDetails {
        /// <summary>
        /// The name of the bank the consumer should wire the amount to.
        /// </summary>
        public required string BankName { get; set; }

        /// <summary>
        ///  The IBAN the consumer should wire the amount to.
        /// </summary>
        public required string BankAccount { get; set; }

        /// <summary>
        /// The BIC of the bank the consumer should wire the amount to.
        /// </summary>
        public required string BankBic { get; set; }

        /// <summary>
        /// The reference the consumer should use when wiring the amount. Note you should not apply any formatting here; show
        /// it to the consumer as-is.
        /// </summary>
        public required string TransferReference { get; set; }

        /// <summary>
        /// Only available if the payment has been completed � The consumer's name.
        /// </summary>
        public string? ConsumerName { get; set; }

        /// <summary>
        /// Only available if the payment has been completed � The consumer's bank account. This may be an IBAN, or it may be a
        /// domestic account number.
        /// </summary>
        public string? ConsumerAccount { get; set; }

        /// <summary>
        /// Only available if the payment has been completed � The consumer's bank's BIC / SWIFT code.
        /// </summary>
        public string? ConsumerBic { get; set; }

        /// <summary>
        /// Only available if filled out in the API or by the consumer � The email address which the consumer asked the payment
        /// instructions to be sent to.
        /// </summary>
        public string? BillingEmail { get; set; }

        /// <summary>
        /// Include a QR code object. Only available for iDEAL, Bancontact and bank transfer payments.
        /// </summary>
        public QrCode? QrCode { get; set; }
    }

    public record BankTransferPaymentResponseLinks : PaymentResponseLinks {
        /// <summary>
        /// A link to a hosted payment page where your customer can check the status of their payment.
        /// </summary>
        public required UrlLink Status { get; set; }

        /// <summary>
        /// A link to a hosted payment page where your customer can finish the payment using an alternative payment method also
        /// activated on your website profile.
        /// </summary>
        public required UrlLink PayOnline { get; set; }
    }
}
