namespace Mollie.Api.Models.Payment.Response {
    public record QrCode {
        /// <summary>
        ///     Height of the image in pixels.
        /// </summary>
        public required int Height { get; init; }

        /// <summary>
        ///     Width of the image in pixels.
        /// </summary>
        public required int Width { get; init; }

        /// <summary>
        ///     The URI you can use to display the QR code. Note that we can send both data URIs as well as links to HTTPS images.
        ///     You should support both.
        /// </summary>
        public required string Src { get; init; }
    }
}