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

        public async Task<InvoiceResponse> GetInvoiceAsync(string invoiceId, bool includeLines = false, bool includeSettlements = false) {
            this.ValidateRequiredUrlParameter(nameof(invoiceId), invoiceId);
            var includes = this.BuildIncludeParameter(includeLines, includeSettlements);
            return await this.GetAsync<InvoiceResponse>($"invoices/{invoiceId}{includes.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(UrlObjectLink<InvoiceResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(string reference = null, int? year = null, string from = null, int? limit = null, bool includeLines = false, bool includeSettlements = false) {
            var parameters = this.BuildIncludeParameter(includeLines, includeSettlements);
            parameters.AddValueIfNotNullOrEmpty(nameof(reference), reference);
            parameters.AddValueIfNotNullOrEmpty(nameof(year), Convert.ToString(year));

            return await this.GetListAsync<ListResponse<InvoiceResponse>>($"invoices", from, limit, parameters).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildIncludeParameter(bool includeLines = false, bool includeSettlements = false) {
            var result = new Dictionary<string, string>();

            var includeList = new List<string>();
            if (includeLines) {
                includeList.Add("lines");
            }

            if (includeSettlements) {
                includeList.Add("settlements");
            }

            if (includeList.Any()) {
                result.Add("include", string.Join(",", includeList));
            }

            return result;
        }
    }
}