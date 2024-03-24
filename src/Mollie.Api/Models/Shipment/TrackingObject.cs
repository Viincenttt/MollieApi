namespace Mollie.Api.Models.Shipment{
    public class TrackingObject {
        /// <summary>
        /// Name of the postal carrier (as specific as possible). For example PostNL.
        /// </summary>
        public required string Carrier { get; init; }

        /// <summary>
        /// The track and trace code of the shipment. For example 3SKABA000000000.
        /// </summary>
        public required string Code { get; init; }

        /// <summary>
        /// The URL where your customer can track the shipment, for example: http://postnl.nl/tracktrace/?B=3SKABA000000000&P=1016EE&D=NL&T=C.
        /// </summary>
        public string? Url { get; set; }
    }
}
