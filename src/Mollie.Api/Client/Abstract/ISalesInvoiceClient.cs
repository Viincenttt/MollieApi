using System;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Api.Models.SalesInvoice.Response;

namespace Mollie.Api.Client.Abstract;

public interface ISalesInvoiceClient : IDisposable {
    Task<SalesInvoiceResponse> CreateSalesInvoiceAsync(SalesInvoiceRequest salesInvoiceRequest);
    Task<ListResponse<SalesInvoiceResponse>> GetSalesInvoiceListAsync(string? from = null, int? limit = null);
    Task<SalesInvoiceResponse> GetSalesInvoiceAsync(string salesInvoiceId);
    Task<SalesInvoiceResponse> UpdateSalesInvoiceAsync(string salesInvoiceId, SalesInvoiceUpdateRequest salesInvoiceRequest);
    Task DeleteSalesInvoiceAsync(string salesInvoiceId);
}
