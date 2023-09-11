namespace Mollie.Api.Models.ClientLink.Request
{
    public class ClientLinkRequest
    {
        /// <summary>
        /// Personal data of your customer which is required for this endpoint.
        /// </summary>
        public ClientLinkOwner Owner { get; set; }
        
        /// <summary>
        /// Name of the organization.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Address of the organization. Note that the country parameter
        /// must always be provided.
        /// </summary>
        public AddressObject Address { get; set; }
        
        /// <summary>
        /// The Chamber of Commerce (or local equivalent) registration number
        /// of the organization.
        /// </summary>
        public string RegistrationNumber { get; set; }
        
        /// <summary>
        /// The VAT number of the organization, if based in the European Union
        /// or the United Kingdom.
        /// </summary>
        public string VatNumber { get; set; }
    }
}