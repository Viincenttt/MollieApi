namespace Mollie.Api.Models.PaymentMethod.Response {
    /// <summary>
    /// URLs of images representing the payment method.
    /// </summary>
    public record PaymentMethodResponseImage {
        /// <summary>
        /// The URL for a payment method icon of 55x37 pixels.
        /// </summary>
        public required string Size1x { get; set; }

        /// <summary>
        /// The URL for a payment method icon of 110x74 pixels. Use this for high resolution screens.
        /// </summary>
        public required string Size2x { get; set; }

        /// <summary>
        /// The URL for a payment method icon in vector format. Usage of this format is preferred since it can scale to any desired size.
        /// </summary>
        public required string Svg { get; set; }

        public override string ToString() {
            return Size1x;
        }
    }
}
