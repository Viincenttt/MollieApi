using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Mollie.Api.Models.Payment.Request {
    public record PaymentRequest
    {
        /// <summary>
        /// The amount that you want to charge, e.g. {"currency":"EUR", "value":"100.00"} if you would want to charge €100.00.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The description of the payment you’re creating. This will be shown to the consumer on their card or bank statement
        /// when possible. We truncate the description automatically according to the limits of the used payment method. The
        /// description is also visible in any exports you generate. We recommend you use a unique identifier so that you can
        /// always link the payment to the order.This is particularly useful for bookkeeping.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Required - The URL the consumer will be redirected to after the payment process. It could make sense for the
        /// redirectURL to contain a unique
        /// identifier – like your order ID – so you can show the right page referencing the order when the consumer returns.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The URL your consumer will be redirected to when the consumer explicitly cancels the payment. If this URL is not
        /// provided, the consumer will be redirected to the redirectUrl instead — see above.

        /// Mollie will always give you status updates via webhooks, including for the canceled status. This parameter is
        /// therefore entirely optional, but can be useful when implementing a dedicated consumer-facing flow to handle payment
        /// cancellations.
        /// </summary>
        public string? CancelUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send payment status updates to.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Allows you to preset the language to be used in the payment screens shown to the consumer. Setting a locale is highly
        /// recommended and will greatly improve your conversion rate. When this parameter is omitted, the browser language will
        /// be used instead if supported by the payment method. You can provide any ISO 15897 locale, but our payment screen currently
        /// only supports the following languages: en_US nl_NL nl_BE fr_FR fr_BE de_DE de_AT de_CH es_ES ca_ES pt_PT it_IT nb_NO
        /// sv_SE fi_FI da_DK is_IS hu_HU pl_PL lv_LV lt_LT
        /// </summary>
        public string? Locale { get; set; }

        /// <summary>
        /// Normally, a payment method selection screen is shown. However, when using this parameter, your customer will skip the
        /// selection screen and will be sent directly to the chosen payment method. The parameter enables you to fully integrate
        /// the payment method selection into your website, however note Mollie’s country based conversion optimization is lost.
        /// See the Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
        /// </summary>
        [JsonIgnore]
        public string? Method {
            get => Methods?.FirstOrDefault();
            set {
                if (value == null) {
                    Methods = null;
                }
                else {
                    Methods = new List<string>();
                    Methods.Add(value);
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
        public IList<string>? Methods { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the payment. Whenever
        /// you fetch the payment with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// Indicate which type of payment this is in a recurring sequence. If set to first, a first payment is created for the
        /// customer, allowing the customer to agree to automatic recurring charges taking place on their account in the future.
        /// If set to recurring, the customer’s card is charged automatically. Defaults to oneoff, which is a regular non-recurring
        /// payment(see also: Recurring). See the Mollie.Api.Models.Payment.SequenceType class for a full list of known values.
        /// </summary>
        public string? SequenceType { get; set; }

        /// <summary>
        /// The ID of the Customer for whom the payment is being created. This is used for recurring payments and single click payments.
        /// </summary>
        public string? CustomerId { get; set; }

        /// <summary>
        /// When creating recurring payments, the ID of a specific Mandate may be supplied to indicate which of the consumer’s accounts
        /// should be credited.
        /// </summary>
        public string? MandateId { get; set; }

		/// <summary>
		///	Oauth only - The payment profile's unique identifier, for example pfl_3RkSN1zuPE. This field is mandatory.
		/// </summary>
		public string? ProfileId { get; set; }

		/// <summary>
		///	Oauth only - Optional – Set this to true to make this payment a test payment.
		/// </summary>
		public bool? Testmode { get; set; }

        /// <summary>
        /// Id of the physical POS terminal that will be used for the payment.
        /// Only required when method = POS
        /// </summary>
        public string? TerminalId { get; set; }

		/// <summary>
		///	Oauth only - Optional – Adding an Application Fee allows you to charge the merchant a small sum for the payment and transfer
		/// this to your own account.
		/// </summary>
		public ApplicationFee? ApplicationFee { get; set; }

        /// <summary>
        /// Oauth only - Optional - An optional routing configuration which enables you to route a successful payment, or part of the payment, to one or more connected accounts.
        /// Additionally, you can schedule (parts of) the payment to become available on the connected account on a future date.
        /// </summary>
        [JsonProperty("routing")]
        public IList<PaymentRoutingRequest>? Routings { get; set; }

        /// <summary>
        /// For digital goods in most jurisdictions, you must apply the VAT rate from your customer’s country. Choose the VAT rates
        /// you have used for the order to ensure your customer’s country matches the VAT country. Use this parameter to restrict the
        /// payment methods available to your customer to those from a single country. If available, the credit card method will still
        /// be offered, but only cards from the allowed country are accepted.
        /// </summary>
        public string? RestrictPaymentMethodsToCountry { get; set; }

        /// <summary>
        /// Indicates whether the capture will be scheduled automatically or not. Set to manual to capture the payment manually using the
        /// Create capture endpoint. Set to automatic by default, which indicates the payment will be captured automatically, without
        /// having to separately request it. Setting automatic without a captureDelay will result in a regular payment.
        /// See the Mollie.Api.Models.Capture.CaptureMode class for a full list of known values.
        /// </summary>
        public string? CaptureMode { get; set; }

        /// <summary>
        /// Interval to wait before the payment is captured, for example 8 hours or 2 days. In order to schedule an automatic capture,
        /// the captureMode must be set to either automatic or be omitted.
        /// Possible values: ... hours ... days
        /// </summary>
        public string? CaptureDelay { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }

        public override string ToString() {
            return $"Method: {Method} - Amount: {Amount}";
        }
    }
}
