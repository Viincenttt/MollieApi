using Newtonsoft.Json;
using System.Collections.Generic;
using Mollie.Api.Models.Issuer;
using Mollie.Api.Models.PaymentMethod.Pricing;

namespace Mollie.Api.Models.PaymentMethod {
    public class PaymentMethodResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a method object. Will always contain method for this endpoint.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The unique identifier of the payment method. When used during payment creation, the payment method selection screen will be skipped.
        /// </summary>
        public Payment.PaymentMethod Id { get; set; }

        /// <summary>
        /// The full name of the payment method.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URLs of images representing the payment method.
        /// </summary>
        public PaymentMethodResponseImage Image { get; set; }

		/// <summary>
		///	List of Issuers
		/// </summary>
		public List<IssuerResponse> Issuers { get; set; }

        /// <summary>
        /// Pricing set of the payment method what will be include if you add the parameter.
        /// </summary>
        public List<PricingResponse> Pricing { get; set; }

		/// <summary>
		/// An object with several URL objects relevant to the payment method. Every URL object will contain an href and a type field.
		/// </summary>
		[JsonProperty("_links")]
        public PaymentMethodResponseLinks Links { get; set; }

        public override string ToString() {
            return this.Description;
        }
    }
}