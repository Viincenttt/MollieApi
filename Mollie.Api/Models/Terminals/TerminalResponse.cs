using System;
using System.Collections.Generic;
using System.Text;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Payment.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Terminals
{
    /// <summary>
    /// Full documentation for this class can be found at https://docs.mollie.com/reference/v2/terminals-api/overview
    /// </summary>
    public class TerminalResponse : IResponseObject
    {
        /// <summary>
        /// The unique identifier used for referring to a terminal. Mollie assigns this identifier at terminal creation time. 
        /// For example term_7MgL4wea46qkRcoTZjWEH. This ID will be used by Mollie to refer to a certain terminal and will be used for assigning a payment to a specific terminal.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The identifier used for referring to the profile the terminal was created on. For example, pfl_QkEhN94Ba.
        /// </summary>
        public string ProfileId { get; set; }
        /// <summary>
        /// The status of the terminal, which is a read-only value determined by Mollie, according to the actions performed for that terminal. 
        /// Its values can be pending, active, inactive. pending means that the terminal has been created but not yet active. 
        /// active means that the terminal is active and can take payments. inactive means that the terminal has been deactivated.
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// The brand of the terminal.
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// The model of the terminal.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// The serial number of the terminal. The serial number is provided at terminal creation time.
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// An ISO 4217 currency code. The currencies supported depend on the payment methods that are enabled on your account.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// The full name of the payment terminal.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The timezone of the terminal. Example: “Europe/Brussels” or "GMT +08:00".
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// Allows you to preset the language to be used in the payment screens shown to the consumer. Setting a locale is highly 
        /// recommended and will greatly improve your conversion rate. When this parameter is omitted, the browser language will 
        /// be used instead if supported by the payment method. You can provide any ISO 15897 locale, but our payment screen currently
        /// only supports the following languages: en_US nl_NL nl_BE fr_FR fr_BE de_DE de_AT de_CH es_ES ca_ES pt_PT it_IT nb_NO 
        /// sv_SE fi_FI da_DK is_IS hu_HU pl_PL lv_LV lt_LT
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        /// The Terminal's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// The Terminal's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// The Terminal's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime? ActivatedAt { get; set; }
        /// <summary>
        /// An object with several URL objects relevant to the payment method. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public TerminalResponseLinks Links { get; set; }
    }
}
