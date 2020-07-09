using Mollie.Api.Framework;

namespace Mollie.Tests.Integration {
    public class MollieIntegationTestConfiguration : MollieConfiguration {
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
