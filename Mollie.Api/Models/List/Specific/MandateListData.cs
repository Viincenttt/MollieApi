using System.Collections.Generic;
using Mollie.Api.Models.Mandate;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class MandateListData : IListData<MandateResponse> {
        [JsonProperty("mandates")]
        public List<MandateResponse> Items { get; set; }
    }
}