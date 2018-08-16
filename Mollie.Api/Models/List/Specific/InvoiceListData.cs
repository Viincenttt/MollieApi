using System.Collections.Generic;
using Mollie.Api.Models.Invoice;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class InvoiceListData : IListData<InvoiceResponse> {
        [JsonProperty("invoices")]
        public List<InvoiceResponse> Items { get; set; }
    }
}