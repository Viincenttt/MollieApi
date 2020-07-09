namespace Mollie.Api.Framework {
    public class MollieConfiguration {
        /// <summary>
        /// The Mollie API key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Client id - only necessery when using the Connect API
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret - only necessery when using the Connect API
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
