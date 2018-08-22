﻿namespace Mollie.Api.Models.Issuer {
	public class IssuerResponse {
		/// <summary>
		/// Contains "issuer"
		/// </summary>
		public string Resource { get; set; }

		/// <summary>
		///     The issuer's unique identifier, for example ideal_ABNANL2A. When creating a payment, specify this ID as the issuer
		///     parameter to forward
		///     the consumer to their banking environment directly.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///     The issuer's full name, for example 'ABN AMRO'.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     Different Issuer Image icons (iDEAL).
		/// </summary>
		public IssuerResponseImage Image { get; set; }
	}
}