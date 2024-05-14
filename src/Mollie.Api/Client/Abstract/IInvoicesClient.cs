using System.Threading.Tasks;
using Mollie.Api.Models.Invoice.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IInvoicesClient : IBaseMollieClient {
        Task<InvoiceResponse> GetInvoiceAsync(string invoiceId);
        Task<InvoiceResponse> GetInvoiceAsync(UrlObjectLink<InvoiceResponse> url);
        Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(
            string? reference = null, int? year = null, string? from = null, int? limit = null);
        Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(UrlObjectLink<ListResponse<InvoiceResponse>> url);
    }
}
