namespace Mollie.Api.Models.Profile.Response {
    public class ProfileResponseLinks {
        public UrlObject Self { get; set; }
        public UrlObject Chargebacks { get; set; }
        public UrlObject Methods { get; set; }
        public UrlObject Payments { get; set; }
        public UrlObject Refunds { get; set; }
        public UrlObject CheckoutPreviewUrl { get; set; }
        public UrlObject Documentation { get; set; }
    }
}