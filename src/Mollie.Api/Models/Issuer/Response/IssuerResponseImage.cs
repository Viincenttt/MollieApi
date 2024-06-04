namespace Mollie.Api.Models.Issuer.Response {
	/// <summary>
	/// URLs of images representing the issuer.
	/// </summary>
	public record IssuerResponseImage {
		public required string Size1x { get; set; }
		public required string Size2x { get; set; }
		public required string Svg { get; set; }

		public override string ToString() {
			return Size1x;
		}
	}
}
