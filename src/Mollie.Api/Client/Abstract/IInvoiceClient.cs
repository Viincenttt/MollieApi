using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Invoice.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IInvoiceClient : IBaseMollieClient {
        Task<MollieResult<InvoiceResponse>> GetInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default);
        Task<MollieResult<InvoiceResponse>> GetInvoiceAsync(UrlObjectLink<InvoiceResponse> url, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<InvoiceResponse>>> GetInvoiceListAsync(
            string? reference = null, int? year = null, string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<InvoiceResponse>>> GetInvoiceListAsync(UrlObjectLink<ListResponse<InvoiceResponse>> url, CancellationToken cancellationToken = default);
    }
}
