namespace Mollie.Api.Models.Profile.Request {
    public record EnableVoucherIssuerRequest {
        /// <summary>
        /// When enabling a voucher issuer, an in-between party may be involved which you have a contract with.
        /// Provide the contract ID the first time you enable an issuer via this contractor. You can update the
        /// contract ID as long as it is not approved yet, by repeating the call with a different contract ID.
        /// </summary>
        public string? ContractId { get; set; }
    }
}
