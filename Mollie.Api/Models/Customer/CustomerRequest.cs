using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Customer {
    public class CustomerRequest {
        /// <summary>
        /// Required - The full name of the customer.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Required - The email address of the customer.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Optional - Allow you to preset the language to be used in the payment screens shown to the consumer. When this parameter is not 
        /// provided, the browser language will be used instead (which is usually more accurate). The input formats are: en (language) or 
        /// en_US (language and region).
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        /// Optional - Provide any data you like in JSON notation, and we will save the data alongside the customer. Whenever you fetch the 
        /// customer with our API, we'll also include the metadata. You can use up to 1kB of JSON.
        /// </summary>
        public string Metadata { get; set; }
    }
}
