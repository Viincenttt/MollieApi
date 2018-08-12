using System.Collections.Generic;
using Mollie.Api.Models.Invoice;

namespace Mollie.Api.Models.List.Specific {
    public class InvoiceListData {
        public List<InvoiceResponse> Invoices { get; set; }
    }
}