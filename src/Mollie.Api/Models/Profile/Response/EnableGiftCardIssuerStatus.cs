namespace Mollie.Api.Models.Profile.Response {
    /// <summary>
    /// The status that the issuer is in. Possible values: pending-issuer or activated.
    /// </summary>
    public static class EnableGiftCardIssuerStatus {
        /// <summary>
        /// The issuer is activated and ready for use.
        /// </summary>
        public const string Activated = "activated";

        /// <summary>
        /// Activation of this issuer relies on you taking action with the issuer itself.
        /// </summary>
        public const string PendingIssuer = "pending-issuer";
    }
}
