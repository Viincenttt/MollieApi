using System.Collections.Generic;

namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public record ManageOrderLinesRequest {
        /// <summary>
        /// List of operations to be processed.
        /// </summary>
        public required IList<ManageOrderLinesOperation> Operations { get; init; }
    }
}