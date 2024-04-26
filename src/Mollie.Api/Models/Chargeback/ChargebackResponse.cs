using System;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Chargeback {
    public record ChargebackResponse : IResponseObject {
		/// <summary>
		/// The chargeback's unique identifier, for example chb_n9z0tp.
		/// </summary>
		public required string Id { get; init; }

        /// <summary>
        /// The amount charged back.
        /// </summary>
        public required Amount Amount { get; init; }

        /// <summary>
        /// This optional field will contain the amount that will be deducted from your account, converted to the currency 
        /// your account is settled in. It follows the same syntax as the amount property.
        /// </summary>
        public Amount? SettlementAmount { get; set; }

        /// <summary>
        /// The date and time the chargeback was issued, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; init; }
        
        /// <summary>
        /// The date and time the chargeback was reversed, in ISO 8601 format.
        /// </summary>
        public DateTime? ReversedAt { get; set; }

        /// <summary>
        /// The id of the payment this chargeback belongs to.
        /// </summary>
		public required string PaymentId { get; init; }
	    
		/// <summary>
        /// The reason given for a Chargeback, this can help determine the cost for the chargeback
        /// </summary>
        public ChargebackResponseReason? Reason { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the chargeback. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required ChargebackResponseLinks Links { get; init; }
    }
}
