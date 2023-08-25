using System;

namespace Mollie.Api.Models.Onboarding.Request {
    /// <summary>
    /// Data of the payment profile you want to provide.
    /// </summary>
    public class OnboardingProfileRequest {
        /// <summary>
        /// The profile’s name should reflect the tradename or brand name of the profile’s website or application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL to the profile’s website or application. The URL must be compliant to RFC3986 with the exception
        /// that we only accept URLs with http:// or https:// schemes and domains that contain a TLD. URLs containing 
        /// an @ are not allowed.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The email address associated with the profile’s tradename or brand.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A description of what kind of goods and/or products will be offered via the payment profile.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The phone number associated with the profile’s trade name or brand. Must be in the E.164 format. For example 
        /// +31208202070.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The industry associated with the profile’s trade name or brand. Please refer to the documentation of the business category
        /// for more information on which values are accepted.
        /// </summary>
        public string BusinessCategory { get; set; }
    }
}
