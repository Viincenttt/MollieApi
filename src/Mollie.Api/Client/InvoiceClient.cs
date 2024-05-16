using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Invoice.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class InvoiceClient : OauthBaseMollieClient, IInvoiceClient {
        public InvoiceClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(string invoiceId) {
            ValidateRequiredUrlParameter(nameof(invoiceId), invoiceId);
            return await GetAsync<InvoiceResponse>($"invoices/{invoiceId}").ConfigureAwait(false);
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(UrlObjectLink<InvoiceResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(
            string? reference = null, int? year = null, string? from = null, int? limit = null) {
            var parameters = new Dictionary<string, string>();
            parameters.AddValueIfNotNullOrEmpty(nameof(reference), reference);
            parameters.AddValueIfNotNullOrEmpty(nameof(year), Convert.ToString(year));

            return await GetListAsync<ListResponse<InvoiceResponse>>($"invoices", from, limit, parameters)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(UrlObjectLink<ListResponse<InvoiceResponse>> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }
    }
}
