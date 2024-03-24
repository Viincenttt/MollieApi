using Newtonsoft.Json;
using System;

namespace Mollie.Api.Models.Onboarding.Response {
    public class OnboardingStatusResponse {
        /// <summary>
        /// Indicates the response contains an onboarding object. Will always contain onboarding for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        /// The name of the organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sign up date and time of the organization.
        /// </summary>
        public DateTime SignedUpAt { get; set; }

        /// <summary>
        /// The current status of the organization’s onboarding process. See the Mollie.Api.Models.Onboarding.Response.OnboardingStatus
        /// class for a full list of known values.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Whether or not the organization can receive payments.
        /// </summary>
        public bool CanReceivePayments { get; set; }

        /// <summary>
        /// Whether or not the organization can receive settlements.
        /// </summary>
        public bool CanReceiveSettlements { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the organization. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public OnboardingStatusResponseLinks Links { get; set; }
    }
}
