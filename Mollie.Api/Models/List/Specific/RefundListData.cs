using System.Collections.Generic;
using Mollie.Api.Models.Refund;

namespace Mollie.Api.Models.List.Specific {
    public class RefundListData {
        public List<RefundResponse> Refunds { get; set; }
    }
}