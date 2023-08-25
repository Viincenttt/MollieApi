namespace Mollie.Api.Models {
    public class CompanyObject {
        /// <summary>
        /// Organization’s registration number.
        /// </summary>
        public string RegistrationNumber { get; set; }
        
        /// <summary>
        /// Organization’s VAT number.
        /// </summary>
        public string VatNumber { get; set; }
        
        /// <summary>
        /// Organization’s entity type.
        /// The Mollie.Api.Models.CompanyEntityType class contains a full list of possible values
        /// </summary>
        public string EntityType { get; set; }
    }
}