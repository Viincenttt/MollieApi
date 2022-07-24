namespace Mollie.Api.Models.Order {
    public class OrderPaymentRequest {
        /// <summary>
        /// Normally, a payment method screen is shown. However, when using this parameter, you can choose a 
        /// specific payment method and your customer will skip the selection screen and is sent directly to 
        /// the chosen payment method. The parameter enables you to fully integrate the payment method 
        /// selection into your website. See the Mollie.Api.Models.Payment.PaymentMethod class for a full 
        /// list of known values.
        /// </summary>
        public string Method { get; set; }

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
