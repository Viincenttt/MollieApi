using System;

namespace Mollie.Api.Models.Profile.Request {
    public class ProfileRequest {
        /// <summary>
        /// The profile's name should reflect the tradename or brand name of the profile's website or application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL to the profile's website or application. The URL should start with http:// or https://.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// The email address associated with the profile's tradename or brand.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The phone number associated with the profile's tradename or brand.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The industry associated with the profile’s trade name or brand. Please refer to the documentation of the business category
        /// for more information on which values are accepted.
        /// </summary>
        public string BusinessCategory { get; set; }
        
        /// <summary>
        /// The industry associated with the profile's tradename or brand. See the Mollie.Api.Models.Profile.CategoryCode class 
        /// for a full list of known values.
        /// </summary>
        [Obsolete("This parameter is deprecated and will be removed in 2022. Please use the businessCategory parameter instead.")]
        public int CategoryCode { get; set; }
        
        /// <summary>
        /// Optional – Creating a test profile by setting this parameter to test, enables you to start using the API without
        /// having to provide all your business info just yet. Defaults to live.
        /// </summary>
        public Mode? Mode { get; set; }
    }
}