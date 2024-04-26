using System;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Terminal
{
    /// <summary>
    /// Full documentation for this class can be found at https://docs.mollie.com/reference/v2/terminals-api/overview
    /// </summary>
    public record TerminalResponse
    {
        /// <summary>
        /// The unique identifier used for referring to a terminal. Mollie assigns this identifier at terminal creation time. 
        /// For example term_7MgL4wea46qkRcoTZjWEH. This ID will be used by Mollie to refer to a certain terminal and will be used for assigning a payment to a specific terminal.
        /// </summary>
        public required string Id { get; init; }
        
        /// <summary>
        /// The identifier used for referring to the profile the terminal was created on. For example, pfl_QkEhN94Ba.
        /// </summary>
        public required string ProfileId { get; init; }
        
        /// <summary>
        /// The status of the terminal, which is a read-only value determined by Mollie, according to the actions performed for that terminal. 
        /// Its values can be pending, active, inactive. pending means that the terminal has been created but not yet active. 
        /// active means that the terminal is active and can take payments. inactive means that the terminal has been deactivated.
        /// </summary>
        public required string Status { get; init; }
        
        /// <summary>
        /// The brand of the terminal.
        /// </summary>
        public required string Brand { get; init; }
        
        /// <summary>
        /// The model of the terminal.
        /// </summary>
        public required string Model { get; init; }
        
        /// <summary>
        /// The serial number of the terminal. The serial number is provided at terminal creation time.
        /// </summary>
        public required string SerialNumber { get; init; }
        
        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public required string Currency { get; init; }
        
        /// <summary>
        /// The full name of the payment terminal.
        /// </summary>
        public required string Description { get; init; }
        
        /// <summary>
        /// The Terminal's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// The Terminal's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// An object with several URL objects relevant to the payment method. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required TerminalResponseLinks Links { get; init; }
    }
}
