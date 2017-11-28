namespace Mollie.Api.Models.Profile {
    public enum ReviewStatus {
        /// <summary>
        ///     The changes are pending review. We will review your changes soon.
        /// </summary>
        Pending,

        /// <summary>
        ///     We've reviewed and rejected your changes.
        /// </summary>
        Rejected
    }
}