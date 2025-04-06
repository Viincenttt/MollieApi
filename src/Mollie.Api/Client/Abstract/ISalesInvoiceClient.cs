using System;
using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Api.Models.SalesInvoice.Response;

namespace Mollie.Api.Client.Abstract;

public interface ISalesInvoiceClient : IDisposable {
    Task<SalesInvoiceResponse> CreateSalesInvoiceAsync(
        SalesInvoiceRequest salesInvoiceRequest, CancellationToken cancellationToken = default);
    Task<ListResponse<SalesInvoiceResponse>> GetSalesInvoiceListAsync(
        string? from = null, int? limit = null, CancellationToken cancellationToken = default);
    Task<SalesInvoiceResponse> GetSalesInvoiceAsync(
        string salesInvoiceId, CancellationToken cancellationToken = default);
    Task<SalesInvoiceResponse> UpdateSalesInvoiceAsync(
        string salesInvoiceId, SalesInvoiceUpdateRequest salesInvoiceRequest, CancellationToken cancellationToken = default);
    Task DeleteSalesInvoiceAsync(string salesInvoiceId, CancellationToken cancellationToken = default);
}
