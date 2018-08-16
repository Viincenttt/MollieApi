using System.Collections.Generic;
using Mollie.Api.Models.PaymentMethod;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class PaymentMethodListData : IListData<PaymentMethodResponse> {
        [JsonProperty("methods")]
        public List<PaymentMethodResponse> Items { get; set; }
    }
}