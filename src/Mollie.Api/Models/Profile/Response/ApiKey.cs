using System;

namespace Mollie.Api.Models.Profile.Response {
    public class ApiKey {
        /// <summary>
        ///     Indicates the response contains an API key object.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        ///     The API key's identifier.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        ///     The actual API key, which you'll use when creating payments or when otherwise communicating with the API. Never
        ///     share the API key with anyone.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     The API key's date and time of creation.
        /// </summary>
        public DateTime CreatedDatetime { get; set; }
    }
}