namespace Mollie.Api.Models.ClientLink.Request
{
    public record ClientLinkOwner
    {
        /// <summary>
        /// The email address of your customer.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The given name (first name) of your customer.
        /// </summary>
        public required string GivenName { get; set; }

        /// <summary>
        /// The family name (surname) of your customer.
        /// </summary>
        public required string FamilyName { get; set; }

        /// <summary>
        /// Allows you to preset the language to be used in the login / authorize flow. When this parameter is
        /// omitted, the browser language will be used instead. You can provide any xx_XX format ISO 15897 locale,
        /// but the authorize flow currently only supports the following languages:
        /// en_US nl_NL nl_BE fr_FR fr_BE de_DE es_ES it_IT
        /// </summary>
        public string? Locale { get; set; }
    }
}
