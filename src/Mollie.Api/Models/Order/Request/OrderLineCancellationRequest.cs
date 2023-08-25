using System.Collections.Generic;

namespace Mollie.Api.Models.Order {
    public class OrderLineCancellationRequest {
        public IEnumerable<OrderLineDetails> Lines { get; set; }
        
        /// <summary>
        ///	Oauth only - Optional
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
