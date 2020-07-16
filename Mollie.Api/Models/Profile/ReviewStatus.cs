namespace Mollie.Api.Models.Profile {
    public static class ReviewStatus {
        /// <summary>
        /// The changes are pending review. We will review your changes soon.
        /// </summary>
        public const string Pending = "pending";

        /// <summary>
        /// We've reviewed and rejected your changes.
        /// </summary>
        public const string Rejected = "rejected";
    }
        
}