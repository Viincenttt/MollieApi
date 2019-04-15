using System.Collections.Generic;

namespace Mollie.Api.Models.Order {
    public class OrderLineCancellationRequest {
        public IEnumerable<OrderLineDetails> Lines { get; set; }
    }
}
