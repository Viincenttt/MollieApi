using Mollie.Api.Models.Payment;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mollie.Api.Models.Customer
{
    public class CustomerResponse
    {
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMode Mode { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Metadata { get; set; }

        public DateTime? CreatedDatetime { get; set; }
    }
}
