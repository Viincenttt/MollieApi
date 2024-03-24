using System.Collections.Generic;

namespace Mollie.Api.Models.Profile.Request {
    public class ProfileRequest {
        /// <summary>
        /// The profile's name should reflect the tradename or brand name of the profile's website or application.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// The URL to the profile's website or application. The URL should start with http:// or https://.
        /// </summary>
        public required string Website { get; init; }

        /// <summary>
        /// The email address associated with the profile's tradename or brand.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// The phone number associated with the profile's tradename or brand.
        /// </summary>
        public required string Phone { get; init; }
        
        /// <summary>
        /// The products or services that the profile’s website offers.
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// The list of countries where you expect that the majority of the profile’s customers will live, in ISO 3166-1 alpha-2 format.
        /// </summary>
        public IEnumerable<string>? CountriesOfActivity { get; set; }

        /// <summary>
        /// The industry associated with the profile’s trade name or brand. Please refer to the documentation of the business category
        /// for more information on which values are accepted.
        /// </summary>
        public string? BusinessCategory { get; set; }
        
        /// <summary>
        /// Optional – Creating a test profile by setting this parameter to test, enables you to start using the API without
        /// having to provide all your business info just yet. Defaults to live.
        /// </summary>
        public Mode? Mode { get; set; }
    }
}