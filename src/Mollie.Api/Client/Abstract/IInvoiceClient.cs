using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.Invoice.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IInvoiceClient : IBaseMollieClient {
        Task<InvoiceResponse> GetInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default);
        Task<InvoiceResponse> GetInvoiceAsync(UrlObjectLink<InvoiceResponse> url, CancellationToken cancellationToken = default);
        Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(
            string? reference = null, int? year = null, string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(UrlObjectLink<ListResponse<InvoiceResponse>> url, CancellationToken cancellationToken = default);
    }
}
