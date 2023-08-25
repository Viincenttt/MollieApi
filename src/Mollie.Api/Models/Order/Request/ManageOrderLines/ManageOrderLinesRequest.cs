using System.Collections.Generic;

namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public class ManageOrderLinesRequest {
        /// <summary>
        /// List of operations to be processed.
        /// </summary>
        public IList<ManageOrderLinesOperation> Operations { get; set; }
    }
}