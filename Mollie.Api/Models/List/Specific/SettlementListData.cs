using Mollie.Api.Models.Settlement;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class SettlementListData : IListData<SettlementResponse> {
        [JsonProperty("settlements")]
        public List<SettlementResponse> Items { get; set; }
    }
}