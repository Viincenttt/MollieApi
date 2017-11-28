using System;

namespace Mollie.Api.Models.Organization
{
    public class OrganizationResponse
    {
		/// <summary>
		/// Indicates the response contains a organization object.
		/// </summary>
		public string Resource { get; set; }

		/// <summary>
		/// The organization's identifier, for example org_1234567.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The organization's official name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The email address of the organization.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// The address where the organizations is established.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// The postal code of where the organization is established.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// The name of the city where the organization is established.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// The name of the country where the organization is established.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// The two-letter code of the country where the organization is established.
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// National or international registration type of the organization's legal entity.
		/// </summary>
		public string RegistrationType { get; set; }

		/// <summary>
		/// Registration number of the organization's legal entity.
		/// </summary>
		public string RegistrationNumber { get; set; }

		/// <summary>
		/// Registration date of the organization's legal entity.
		/// </summary>
		public DateTime RegistrationDatetime { get; set; }

		/// <summary>
		/// Date on which Mollie's verification of this organization completed successfully.
		/// </summary>
		public DateTime VerifiedDatetime { get; set; }
    }
}