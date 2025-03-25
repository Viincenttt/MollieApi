using System;
using System.Threading.Tasks;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Api.Models.SalesInvoice.Response;

namespace Mollie.Api.Client.Abstract;

public interface ISalesInvoiceClient : IDisposable {
    Task<SalesInvoiceResponse> CreateSalesInvoiceAsync(SalesInvoiceRequest salesInvoiceRequest);
}
