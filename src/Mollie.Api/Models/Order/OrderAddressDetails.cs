namespace Mollie.Api.Models.Order {
    public record OrderAddressDetails : AddressObject {
        /// <summary>
        /// The person’s organization, if applicable.
        /// </summary>
        public string? OrganizationName { get; set; }

        /// <summary>
        /// The title of the person, for example Mr. or Mrs..
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The given name (first name) of the person.
        /// </summary>
        public string? GivenName { get; set; }

        /// <summary>
        /// The family name (surname) of the person.
        /// </summary>
        public string? FamilyName { get; set; }

        /// <summary>
        /// The email address of the person.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The phone number of the person. Some payment methods require this information. If you have it, you
        /// should pass it so that your customer does not have to enter it again in the checkout. Must be in
        /// the E.164 format. For example +31208202070.
        /// </summary>
        public string? Phone { get; set; }
    }
}