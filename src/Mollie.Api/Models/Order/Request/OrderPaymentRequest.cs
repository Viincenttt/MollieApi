using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Order {
    public class OrderPaymentRequest {
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
        /// The ID of the Customer for whom the payment is being created. This is used for recurring payments
        /// and single click payments.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// When creating recurring payments, the ID of a specific Mandate may be supplied to indicate which
        /// of the consumer’s accounts should be credited.
        /// </summary>
        public string MandateId { get; set; }
        
        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this payment a test payment.
        /// </summary>
        public bool? Testmode { get; set; }
        
        /// <summary>
        ///	Oauth only - Optional – Adding an Application Fee allows you to charge the merchant a small sum for the payment and transfer 
        /// this to your own account.
        /// </summary>
        public ApplicationFee ApplicationFee { get; set; }
    }
}
