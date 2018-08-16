using System.Collections.Generic;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Payment.Response;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class PaymentListData : IListData<PaymentResponse> {
        [JsonProperty("payments")]
        public List<PaymentResponse> Items { get; set; }
    }
}
