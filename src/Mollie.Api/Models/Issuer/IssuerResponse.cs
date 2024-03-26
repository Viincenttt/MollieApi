namespace Mollie.Api.Models.Issuer {
    public class IssuerResponse : IResponseObject {
        /// <summary>
        /// Contains "issuer"
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        ///     The issuer's unique identifier, for example ideal_ABNANL2A. When creating a payment, specify this ID as the issuer
        ///     parameter to forward
        ///     the consumer to their banking environment directly.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        ///     The issuer's full name, for example 'ABN AMRO'.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        ///     Different Issuer Image icons (iDEAL).
        /// </summary>
        public required IssuerResponseImage Image { get; init; }
    }
}