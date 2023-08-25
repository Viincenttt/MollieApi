namespace Mollie.Api.Models.Onboarding.Response {
    /// <summary>
    /// The current status of the organization’s onboarding process
    /// </summary>
    public static class OnboardingStatus {
        /// <summary>
        /// The onboarding is not completed and the merchant needs to provide (more) information
        /// </summary>
        public const string NeedsData = "needs-data";

        /// <summary>
        /// The merchant provided all information and Mollie needs to check this
        /// </summary>
        public const string InReview = "in-review";

        /// <summary>
        /// The onboarding is completed
        /// </summary>
        public const string Completed = "completed";
    }
}
