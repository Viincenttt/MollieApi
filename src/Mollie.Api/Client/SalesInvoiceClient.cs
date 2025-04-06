using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Api.Models.SalesInvoice.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client;

public class SalesInvoiceClient : BaseMollieClient, ISalesInvoiceClient {
    public SalesInvoiceClient(string apiKey, HttpClient? httpClient = null)
        : base(apiKey, httpClient) { }

    [ActivatorUtilitiesConstructor]
    public SalesInvoiceClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient) {
    }

    public async Task<SalesInvoiceResponse> CreateSalesInvoiceAsync(SalesInvoiceRequest salesInvoiceRequest) {
        return await PostAsync<SalesInvoiceResponse>($"sales-invoices", salesInvoiceRequest).ConfigureAwait(false);
    }

    public async Task<SalesInvoiceResponse> GetSalesInvoiceAsync(string salesInvoiceId) {
        return await GetAsync<SalesInvoiceResponse>($"sales-invoices/{salesInvoiceId}").ConfigureAwait(false);
    }
}
