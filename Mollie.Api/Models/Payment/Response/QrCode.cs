namespace Mollie.Api.Models.Payment.Response {
    public class QrCode {
        /// <summary>
        /// Height of the image in pixels.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Width of the image in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The URI you can use to display the QR code. Note that we can send both data URIs as well as links to HTTPS images. You should support both.
        /// </summary>
        public string Src { get; set; }
    }
}
