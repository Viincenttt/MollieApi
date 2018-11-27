﻿using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment.Request {
    public class PaymentRequest {
        /// <summary>
        /// The amount that you want to charge, e.g. {"currency":"EUR", "value":"100.00"} if you would want to charge €100.00.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// The description of the payment you’re creating. This will be shown to the consumer on their card or bank statement 
        /// when possible. We truncate the description automatically according to the limits of the used payment method. The 
        /// description is also visible in any exports you generate. We recommend you use a unique identifier so that you can 
        /// always link the payment to the order.This is particularly useful for bookkeeping.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required - The URL the consumer will be redirected to after the payment process. It could make sense for the
        /// redirectURL to contain a unique
        /// identifier – like your order ID – so you can show the right page referencing the order when the consumer returns.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send payment status updates to.
        /// </summary>
        public string WebhookUrl { get; set; }


        /// <summary>
        /// Allows you to preset the language to be used in the payment screens shown to the consumer. Setting a locale is highly 
        /// recommended and will greatly improve your conversion rate. When this parameter is omitted, the browser language will 
        /// be used instead if supported by the payment method. You can provide any ISO 15897 locale, but our payment screen currently
        /// only supports the following languages: en_US nl_NL nl_BE fr_FR fr_BE de_DE de_AT de_CH es_ES ca_ES pt_PT it_IT nb_NO 
        /// sv_SE fi_FI da_DK is_IS hu_HU pl_PL lv_LV lt_LT
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Normally, a payment method selection screen is shown. However, when using this parameter, your customer will skip the 
        /// selection screen and will be sent directly to the chosen payment method. The parameter enables you to fully integrate 
        /// the payment method selection into your website, however note Mollie’s country based conversion optimization is lost.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMethod? Method { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the payment. Whenever 
        /// you fetch the payment with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Indicate which type of payment this is in a recurring sequence. If set to first, a first payment is created for the 
        /// customer, allowing the customer to agree to automatic recurring charges taking place on their account in the future. 
        /// If set to recurring, the customer’s card is charged automatically. Defaults to oneoff, which is a regular non-recurring 
        /// payment(see also: Recurring).
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SequenceType? SequenceType { get; set; }

        /// <summary>
        /// The ID of the Customer for whom the payment is being created. This is used for recurring payments and single click payments.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// When creating recurring payments, the ID of a specific Mandate may be supplied to indicate which of the consumer’s accounts 
        /// should be credited.
        /// </summary>
        public string MandateId { get; set; }

		/// <summary>
		///	Oauth only - The payment profile's unique identifier, for example pfl_3RkSN1zuPE. This field is mandatory.
		/// </summary>
		public string ProfileId { get; set; }

		/// <summary>
		///	Oauth only - Optional – Set this to true to make this payment a test payment.
		/// </summary>
		public bool? Testmode { get; set; }

		/// <summary>
		///	Oauth only - Optional – Adding an Application Fee allows you to charge the merchant a small sum for the payment and transfer 
		/// this to your own account.
		/// </summary>
		public PaymentRequestApplicationFee ApplicationFee { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings jsonSerializerSettings = null) {
            this.Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }

        public override string ToString() {
            return $"Method: {this.Method} - Amount: {this.Amount}";
        }
    }
}