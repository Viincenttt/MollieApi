using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Invoice;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class InvoicesClient : OauthBaseMollieClient, IInvoicesClient {
        public InvoicesClient(string oauthAccessToken, HttpClient httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(string invoiceId) {
            this.ValidateRequiredUrlParameter(nameof(invoiceId), invoiceId);
            return await this.GetAsync<InvoiceResponse>($"invoices/{invoiceId}").ConfigureAwait(false);
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(UrlObjectLink<InvoiceResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(
            string reference = null, int? year = null, string from = null, int? limit = null) {
            var parameters = new Dictionary<string, string>();
            parameters.AddValueIfNotNullOrEmpty(nameof(reference), reference);
            parameters.AddValueIfNotNullOrEmpty(nameof(year), Convert.ToString(year));

            return await this.GetListAsync<ListResponse<InvoiceResponse>>($"invoices", from, limit, parameters)
                .ConfigureAwait(false);
        }
        
        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(UrlObjectLink<ListResponse<InvoiceResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }
        
        private Dictionary<string, string> BuildQueryParameters(string profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("profileId", profileId);
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}