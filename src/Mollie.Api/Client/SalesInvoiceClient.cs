using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Api.Models.SalesInvoice.Response;
using Mollie.Api.Options;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client;

public class SalesInvoiceClient : BaseMollieClient, ISalesInvoiceClient {
    public SalesInvoiceClient(string apiKey, HttpClient? httpClient = null)
        : base(apiKey, httpClient) { }

    [ActivatorUtilitiesConstructor]
    public SalesInvoiceClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient) {
    }

    public async Task<SalesInvoiceResponse> CreateSalesInvoiceAsync(
        SalesInvoiceRequest salesInvoiceRequest, CancellationToken cancellationToken = default) {
        return await PostAsync<SalesInvoiceResponse>($"sales-invoices", salesInvoiceRequest, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ListResponse<SalesInvoiceResponse>> GetSalesInvoiceListAsync(
        string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
        var queryParameters = BuildQueryParameters(testmode: testmode);
        return await GetListAsync<ListResponse<SalesInvoiceResponse>>("sales-invoices", from, limit, queryParameters, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ListResponse<SalesInvoiceResponse>> GetSalesInvoiceListAsync(
        UrlObjectLink<ListResponse<SalesInvoiceResponse>> url, CancellationToken cancellationToken = default) {
        return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<SalesInvoiceResponse> GetSalesInvoiceAsync(
        string salesInvoiceId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(salesInvoiceId), salesInvoiceId);
        var queryParameters = BuildQueryParameters(testmode: testmode);
        return await GetAsync<SalesInvoiceResponse>($"sales-invoices/{salesInvoiceId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<SalesInvoiceResponse> GetSalesInvoiceAsync(
        UrlObjectLink<SalesInvoiceResponse> url, CancellationToken cancellationToken = default) {
        return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<SalesInvoiceResponse> UpdateSalesInvoiceAsync(
        string salesInvoiceId, SalesInvoiceUpdateRequest salesInvoiceRequest, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(salesInvoiceId), salesInvoiceId);
        return await PatchAsync<SalesInvoiceResponse>($"sales-invoices/{salesInvoiceId}", salesInvoiceRequest, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task DeleteSalesInvoiceAsync(string salesInvoiceId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(salesInvoiceId), salesInvoiceId);
        var data = CreateTestmodeModel(testmode);
        await DeleteAsync($"sales-invoices/{salesInvoiceId}", data, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
