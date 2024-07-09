namespace Mollie.Api.Models {
    public record AddressObject {
        /// <summary>
        /// The title of the person, for example Mr. or Mrs..
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The given name (first name) of the person should be at least two characters and cannot contain only numbers.
        /// </summary>
        public string? GivenName { get; set; }

        /// <summary>
        /// The given family name (surname) of the person should be at least two characters and cannot contain only numbers.
        /// </summary>
        public string? FamilyName { get; set; }

        /// <summary>
        /// The name of the organization, in case the addressee is an organization.
        /// </summary>
        public string? OrganizationName { get; set; }

        /// <summary>
        /// The card holder’s street and street number.
        /// </summary>
        public string? StreetAndNumber { get; set; }

        /// <summary>
        /// Any additional addressing details, for example an apartment number.
        /// </summary>
        public string? StreetAdditional { get; set; }

        /// <summary>
        /// The card holder’s postal code.
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The card holder’s city.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// The card holder’s region.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// The card holder’s country in ISO 3166-1 alpha-2 format.
        /// </summary>
        public string? Country { get; set; }
    }
}
