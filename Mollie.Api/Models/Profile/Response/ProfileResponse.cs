using System;

namespace Mollie.Api.Models.Profile.Response {
    public class ProfileResponse {
        /// <summary>
        ///     The payment profile's unique identifier, for example pfl_3RkSN1zuPE.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Indicates whether the payment profile is in test or production mode.
        ///     Possible values: live or test
        /// </summary>
        public Mode Mode { get; set; }

        /// <summary>
        ///     The payment profile's name, this will usually reflect the tradename or brand name of the profile's website or
        ///     application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The URL to the profile's website or application.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        ///     The email address associated with the profile's tradename or brand.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     The phone number associated with the profile's tradename or brand.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     The industry associated with the profile's tradename or brand.
        /// </summary>
        public CategoryCode CategoryCode { get; set; }

        /// <summary>
        ///     The profile status determines whether the payment profile is able to receive live payments.
        /// </summary>
        public ProfileStatus Status { get; set; }

        /// <summary>
        ///     The presence of a review object indicates changes have been made that have not yet been approved by Mollie.
        ///     Changes to test profiles are approved automatically, unless a switch to a live profile has been requested.
        ///     The review object will therefore usually be null in test mode.
        /// </summary>
        public Review Review { get; set; }

        /// <summary>
        ///     The payment profile's date and time of creation.
        /// </summary>
        public DateTime CreatedDatetime { get; set; }

        /// <summary>
        ///     The date and time of the payment profile's last edit
        /// </summary>
        public DateTime UpdatedDatetime { get; set; }

        /// <summary>
        ///     Useful URLs to related resources.
        /// </summary>
        public Links Links { get; set; }
    }

    public class Links {
        /// <summary>
        ///     The URL to the nested API keys resource.
        /// </summary>
        public string Apikeys { get; set; }

        /// <summary>
        ///     The Checkout preview URL. You need to be logged in to access this page.
        /// </summary>
        public string CheckoutPreviewUrl { get; set; }
    }

    public class Review {
        /// <summary>
        ///     The status of the requested profile changes.
        /// </summary>
        public ReviewStatus Status { get; set; }
    }
}