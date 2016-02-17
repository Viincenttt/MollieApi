namespace Mollie.Api.Models.PaymentMethod {
    /// <summary>
    /// The minimum and maximum allowed payment amount will differ between payment methods.
    /// </summary>
    public class PaymentMethodResponseAmount {
        /// <summary>
        /// The minimum payment amount in EUROs required to use this payment method.
        /// </summary>
        public decimal Minimum { get; set; }

        /// <summary>
        /// The maximum payment amount in EUROs allowed when using this payment method.
        /// </summary>
        public decimal Maximum { get; set; }

        public override string ToString() {
            return $"Minimum: {this.Minimum} - Maximum: {this.Maximum}";
        }
    }
}
