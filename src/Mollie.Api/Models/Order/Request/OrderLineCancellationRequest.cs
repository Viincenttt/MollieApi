using System.Collections.Generic;

namespace Mollie.Api.Models.Order {
    public record OrderLineCancellationRequest {
        public required IEnumerable<OrderLineDetails> Lines { get; init; }
        
        /// <summary>
        ///	Oauth only - Optional
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
