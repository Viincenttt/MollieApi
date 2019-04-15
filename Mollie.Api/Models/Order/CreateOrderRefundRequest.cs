using System.Collections.Generic;

namespace Mollie.Api.Models.Order {
    public class CreateOrderRefundRequest {
        public IEnumerable<OrderLineDetails> Lines { get; set; }
        public string Description { get; set; }
    }
}
