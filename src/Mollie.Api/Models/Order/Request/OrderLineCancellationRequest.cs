using System.Collections.Generic;

namespace Mollie.Api.Models.Order.Request {
    public record OrderLineCancellationRequest : ITestModeRequest {
        public required IEnumerable<OrderLineDetails> Lines { get; set; }

        /// <summary>
        ///	Oauth only - Optional
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
