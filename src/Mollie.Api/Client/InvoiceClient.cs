using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Invoice.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class InvoiceClient : OauthBaseMollieClient, IInvoiceClient {
        public InvoiceClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public InvoiceClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(
            string invoiceId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(invoiceId), invoiceId);
            return await GetAsync<InvoiceResponse>(
                $"invoices/{invoiceId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(
            UrlObjectLink<InvoiceResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(
            string? reference = null, int? year = null, string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            var parameters = new Dictionary<string, string>();
            parameters.AddValueIfNotNullOrEmpty(nameof(reference), reference);
            parameters.AddValueIfNotNullOrEmpty(nameof(year), Convert.ToString(year));

            return await GetListAsync<ListResponse<InvoiceResponse>>(
                    "invoices", from, limit, parameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(
            UrlObjectLink<ListResponse<InvoiceResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
