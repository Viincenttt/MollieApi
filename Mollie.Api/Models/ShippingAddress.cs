using System;
using System.Collections.Generic;
using System.Text;

namespace Mollie.Api.Models
{
    public class ShippingAddress
    {
        /// <summary>
        /// The given name (first name) of the person. The maximum character length of givenName and familyName combined is 128.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// The family name (surname) of the person. The maximum character length of givenName and familyName combined is 128.
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// The street and street number of the shipping address. The maximum character length is 128.
        /// </summary>
        public string StreetAndNumber { get; set; }

        /// <summary>
        /// The postal code of the shipping address. The maximum character length is 20.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// The city of the shipping address. The maximum character length is 100.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The region of the shipping address. The maximum character length is 100. Please note: this field is required if country is one of the following countries: AR BR CA CN ID IN JP MX TH US
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// The country of the shipping address in ISO 3166-1 alpha-2 format.
        /// </summary>
        public string Country { get; set; }
    }
}
