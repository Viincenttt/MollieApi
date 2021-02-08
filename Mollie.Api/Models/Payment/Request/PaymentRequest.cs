using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Mollie.Api.Models.Payment.Request {
    public class PaymentRequest {
        public PaymentRequest() {
        }

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
        /// See the Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
        /// </summary>
        [JsonIgnore]
        public string Method { 
            get {
                return this.Methods.FirstOrDefault();
            }
            set {
                if (value == null) {
                    this.Methods = null;
                }
                else {
                    this.Methods = new List<string>();
                    this.Methods.Add(value);
                }
            }
        }

        /// <summary>
        /// Normally, a payment method screen is shown. However, when using this parameter, you can choose a specific payment method
        /// and your customer will skip the selection screen and is sent directly to the chosen payment method. The parameter 
        /// enables you to fully integrate the payment method selection into your website.
        /// You can also specify the methods in an array.By doing so we will still show the payment method selection screen but will 
        /// only show the methods specified in the array. For example, you can use this functionality to only show payment methods 
        /// from a specific country to your customer.
        /// </summary>
        [JsonProperty("method")]
        public IList<string> Methods { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the payment. Whenever 
        /// you fetch the payment with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        /// <summary>
        /// Indicate which type of payment this is in a recurring sequence. If set to first, a first payment is created for the 
        /// customer, allowing the customer to agree to automatic recurring charges taking place on their account in the future. 
        /// If set to recurring, the customer’s card is charged automatically. Defaults to oneoff, which is a regular non-recurring 
        /// payment(see also: Recurring). See the Mollie.Api.Models.Payment.SequenceType class for a full list of known values.
        /// </summary>
        public string SequenceType { get; set; }

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
		public ApplicationFee ApplicationFee { get; set; }

        /// <summary>
        /// For digital goods in most jurisdictions, you must apply the VAT rate from your customer’s country. Choose the VAT rates 
        /// you have used for the order to ensure your customer’s country matches the VAT country. Use this parameter to restrict the 
        /// payment methods available to your customer to those from a single country. If available, the credit card method will still
        /// be offered, but only cards from the allowed country are accepted.
        /// </summary>
        public string RestrictPaymentMethodsToCountry { get; set; }

        /// <summary>
        /// The shipping address details.We advise to provide these details to improve PayPal’s fraud protection, and thus improve conversion.
        /// The following fields can be added to the object:
        /// </summary>
        public ShippingAddress ShippingAddress { get; set; }	    
	    
        public void SetMetadata(object metadataObj, JsonSerializerSettings jsonSerializerSettings = null) {
            this.Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }

        public override string ToString() {
            return $"Method: {this.Method} - Amount: {this.Amount}";
        }
    }
}
