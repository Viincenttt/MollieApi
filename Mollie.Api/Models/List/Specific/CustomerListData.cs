using System.Collections.Generic;
using Mollie.Api.Models.Customer;

namespace Mollie.Api.Models.List.Specific {
    public class CustomerListData {
        public List<CustomerResponse> Customers { get; set; }
    }
}