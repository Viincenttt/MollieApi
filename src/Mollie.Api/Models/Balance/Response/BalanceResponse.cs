using System;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Balance.Response {
    public class BalanceResponse : IEntity {
        /// <summary>
        /// Indicates the response contains a balance object. Will always contain balance for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The identifier uniquely referring to this balance. Mollie assigns this identifier at balance creation time. For example bal_gVMhHKqSSRYJyPsuoPNFH.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The balance’s date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// The balance’s ISO 4217 currency code.
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// The status of the balance.
        /// </summary>
        public required BalanceResponseStatus Status { get; set; }

        /// <summary>
        /// The frequency at which the available amount on the balance will be settled to the configured transfer destination.
        /// See transferDestination.
        /// </summary>
        public required string TransferFrequency { get; set; }

        /// <summary>
        /// The minimum amount configured for scheduled automatic settlements. As soon as the amount on the balance exceeds this threshold,
        /// the complete balance will be paid out to the transferDestination according to the configured transferFrequency.
        /// </summary>
        public required Amount TransferThreshold { get; set; }

        /// <summary>
        /// The transfer reference set to be included in all the transfers for this balance. Either a string or null.
        /// </summary>
        public string? TransferReference { get; set; }

        /// <summary>
        /// The destination where the available amount will be automatically transferred to according to the configured transferFrequency.
        /// </summary>
        public required BalanceTransferDestination TransferDestination { get; set; }

        /// <summary>
        /// The amount directly available on the balance, e.g. {"currency":"EUR", "value":"100.00"}.
        /// </summary>
        public required Amount AvailableAmount { get; set; }

        /// <summary>
        /// The total amount that is queued to be transferred to your balance. For example, a credit card payment can take a few days to clear.
        /// </summary>
        public required Amount PendingAmount { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the balance. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required BalanceResponseLinks Links { get; set; }
    }
}
