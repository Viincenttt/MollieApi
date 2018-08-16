using Mollie.Api.Models.Chargeback;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class ChargebackListData : IListData<ChargebackResponse> {
        [JsonProperty("chargebacks")]
        public List<ChargebackResponse> Items { get; set; }
    }
}