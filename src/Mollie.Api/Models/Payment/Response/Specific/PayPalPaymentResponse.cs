namespace Mollie.Api.Models.Payment.Response {
    public class PayPalPaymentResponse : PaymentResponse {
        public required PayPalPaymentResponseDetails Details { get; init; }
    }

    public class PayPalPaymentResponseDetails {
        /// <summary>
        /// Only available if the payment has been completed – The consumer’s first and last name.
        /// </summary>
        public string? ConsumerName { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer’s email address.
        /// </summary>
        public string? ConsumerAccount { get; set; }

        /// <summary>
        /// PayPal’s reference for the transaction, for instance 9AL35361CF606152E.
        /// </summary>
        public string PayPalReference { get; set; }

        /// <summary>
        /// ID for the consumer's PayPal account, for instance WDJJHEBZ4X2LY.
        /// </summary>
        public string PaypalPayerId { get; set; }

        /// <summary>
        /// Indicates if the payment is eligible for PayPal's Seller Protection. This parameter is omitted if we did not 
        /// received the information from PayPal. See the Mollie.Api.Models.Payment.Response.PayPalSellerProtection class
        /// for a full list of known values.
        /// </summary>
        public string? SellerProtection { get; set; }

        /// <summary>
        /// The shipping address details.
        /// </summary>
        public AddressObject? ShippingAddress { get; set; }
        
        /// <summary>
        /// The amount of fee PayPal will charge for this transaction. This field is omitted if PayPal will not charge a fee 
        /// for this transaction.
        /// </summary>
        public Amount? PaypalFee { get; set; }
    }

    public static class PayPalSellerProtection {
        public const string Eligible = nameof(Eligible);
        public const string Ineligible = nameof(Ineligible);
        public const string PartiallyEligibleInrOnly = "Partially Eligible - INR Only";
        public const string PartiallyEligibleUnauthOnly = "Partially Eligible - Unauth Only";
        public const string PartiallyEligible = nameof(PartiallyEligible);
        public const string None = nameof(None);
        public const string ActiveFraudControlUnauthPremiumEligible = "Active Fraud Control - Unauth Premium Eligible";
    }
}