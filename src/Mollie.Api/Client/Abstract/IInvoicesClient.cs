using System;
using System.Threading.Tasks;
using Mollie.Api.Models.Invoice;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IInvoicesClient : IDisposable {
        Task<InvoiceResponse> GetInvoiceAsync(string invoiceId, bool includeLines = false, bool includeSettlements = false);
        Task<InvoiceResponse> GetInvoiceAsync(UrlObjectLink<InvoiceResponse> url);
        Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(string reference = null, int? year = null, 
            string from = null, int? limit = null, bool includeLines = false, bool includeSettlements = false);
        Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(UrlObjectLink<ListResponse<InvoiceResponse>> url);
    }
}