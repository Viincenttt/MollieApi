namespace Mollie.Api.Models.Profile.Response {
    public record EnableVoucherIssuerResponseContractor {
        /// <summary>
        /// The unique identifier of the contractor.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The name of the contractor.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The contract ID with the contractor.
        /// </summary>
        public string? ContractId { get; set; }
    }
}
