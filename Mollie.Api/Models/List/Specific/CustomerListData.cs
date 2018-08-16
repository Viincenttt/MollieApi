using System.Collections.Generic;
using Mollie.Api.Models.Customer;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class CustomerListData : IListData<CustomerResponse> {
        [JsonProperty("customers")]
        public List<CustomerResponse> Items { get; set; }
    }
}