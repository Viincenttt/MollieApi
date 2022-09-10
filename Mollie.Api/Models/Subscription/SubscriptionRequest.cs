namespace Mollie.Api.Models.Subscription {
    public class SubscriptionRequest : SubscriptionUpdateRequest {
        /// <summary>
        /// Optional – The payment method used for this subscription, either forced on creation or null if any of the
        /// customer's valid mandates may be used. See the Mollie.Api.Models.Payment.PaymentMethod class for a full
        /// list of known values.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Adding an application fee allows you to charge the merchant for each payment in the subscription and 
        /// transfer these amounts to your own account.
        /// </summary>
        public ApplicationFee ApplicationFee { get; set; }
        
        /// <summary>
        /// The website profile’s unique identifier, for example pfl_3RkSN1zuPE.
        /// </summary>
        public string ProfileId { get; set; }
    }
}