using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mollie.Api.Models.Mandate {
    using Payment;

    public class MandateResponse {
        /// <summary>
        /// Unique identifier of you mandate.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Current status of mandate.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MandateStatus Status { get; set; }

        /// <summary>
        /// DateTime when mandate was created.
        /// </summary>
        public DateTime? CreatedDatetime { get; set; }

        /// <summary>
        /// Payment method of the mandate.
        /// </summary>
        public PaymentMethod Method { get; set; }

        /// <summary>
        /// The customer's unique identifier, for example cst_3RkSN1zuPE.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Mandate details that are different per payment method. Available fields depend on that payment method.
        /// </summary>
        public MandateDetails Details { get; set; }
    }

    public class MandateDetails {
        /// <summary>
        /// The direct debit account holder's name.
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// The direct debit account IBAN.
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// The direct debit account BIC.
        /// </summary>
        public string ConsumerBic { get; set; }

        /// <summary>
        /// The credit card holder's name.
        /// </summary>
        public string CardHolder { get; set; }

        /// <summary>
        /// The last four digits of the credit card number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// The credit card's label. Note that not all labels can be acquired through Mollie.
        /// </summary>
        public string CardLabel { get; set; }

        /// <summary>
        /// Unique alphanumeric representation of credit card, usable for identifying returning customers.
        /// </summary>
        public string CardFingerprint { get; set; }

        /// <summary>
        /// Expiry date of the credit card card in YYYY-MM-DD format.
        /// </summary>
        public string CardExpiryDate { get; set; }
    }
}
