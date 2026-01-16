using System.Collections.Generic;

namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public record ManageOrderLinesRequest : ITestModeRequest {
        /// <summary>
        /// List of operations to be processed.
        /// </summary>
        public required IList<ManageOrderLinesOperation> Operations { get; set; }

        /// <summary>
        ///	Oauth only - Optional
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
