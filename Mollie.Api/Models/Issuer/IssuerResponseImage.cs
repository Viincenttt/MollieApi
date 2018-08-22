namespace Mollie.Api.Models.Issuer {
	/// <summary>
	/// URLs of images representing the issuer.
	/// </summary>
	public class IssuerResponseImage {
		public string Size1x { get; set; }
		public string Size2x { get; set; }
		public string Svg { get; set; }

		public override string ToString()
		{
			return this.Size1x;
		}
	}

}