using System.Collections.Generic;
using Mollie.Api.Models.Refund;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class RefundListData : IListData<RefundResponse> {
        [JsonProperty("refunds")]
        public List<RefundResponse> Items { get; set; }
    }
}