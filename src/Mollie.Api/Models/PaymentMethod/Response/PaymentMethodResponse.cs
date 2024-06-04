﻿using System.Collections.Generic;
using Mollie.Api.Models.Issuer.Response;
using Newtonsoft.Json;

namespace Mollie.Api.Models.PaymentMethod.Response {
    public record PaymentMethodResponse {
        /// <summary>
        /// Indicates the response contains a method object. Will always contain method for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The unique identifier of the payment method. When used during payment creation, the payment method selection screen will be skipped.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The full name of the payment method.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Minimum payment amount required to use this payment method.
        /// </summary>
        public required Amount MinimumAmount { get; set; }

        /// <summary>
        /// Maximum payment amount allowed when using this payment method. (Could be null)
        /// </summary>
        public required Amount MaximumAmount { get; set; }

        /// <summary>
        /// URLs of images representing the payment method.
        /// </summary>
        public required PaymentMethodResponseImage Image { get; set; }

		/// <summary>
		///	List of Issuers
		/// </summary>
		public List<IssuerResponse>? Issuers { get; set; }

        /// <summary>
        /// Pricing set of the payment method what will be include if you add the parameter.
        /// </summary>
        public List<PricingResponse>? Pricing { get; set; }

		/// <summary>
		/// An object with several URL objects relevant to the payment method. Every URL object will contain an href and a type field.
		/// </summary>
		[JsonProperty("_links")]
        public required PaymentMethodResponseLinks Links { get; set; }

        public override string ToString() {
            return Description;
        }
    }
}
