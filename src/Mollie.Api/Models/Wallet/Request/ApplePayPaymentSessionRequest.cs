namespace Mollie.Api.Models.Wallet.Request {
    public class ApplePayPaymentSessionRequest {
        /// <summary>
        /// The validationUrl you got from the ApplePayValidateMerchant event.
        /// A list of all valid host names for merchant validation is available. You should white list these in your
        /// application and reject any validationUrl that have a host name not in the list.
        /// </summary>
        public string ValidationUrl { get; set; }
        
        /// <summary>
        /// The domain of your web shop, that is visible in the browser’s location bar. For example pay.myshop.com.
        /// </summary>
        public string Domain { get; set; }
    }
}