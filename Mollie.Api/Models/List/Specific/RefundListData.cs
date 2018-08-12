using System.Collections.Generic;
using Mollie.Api.Models.Refund;

namespace Mollie.Api.Models.List.Specific {
    public class RefundListData {
        public List<RefundResponse> Chargebacks { get; set; }
    }
}