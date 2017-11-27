using System;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Chargeback
{
    public class ChargebackResponse
    {
		/// <summary>
		/// The chargeback's unique identifier, for example chb_n9z0tp.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The id of the payment this chargeback belongs to.
		/// </summary>
		[JsonProperty(PropertyName = "payment")]
		public string PaymentId { get; set; }

		/// <summary>
		/// The amount charged back.
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// The date and time the chargeback was issued, in ISO 8601 format.
		/// </summary>
		public DateTime ChargebackDatetime { get; set; }

		/// <summary>
		/// The date and time the chargeback was reversed, in ISO 8601 format.
		/// </summary>
		public DateTime? ReversedDatetime { get; set; }
    }
}