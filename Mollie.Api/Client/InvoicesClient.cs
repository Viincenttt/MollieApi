using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Invoice;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client {
    public class InvoicesClient : OauthBaseMollieClient, IInvoicesClient {
        public InvoicesClient(string oauthAccessToken) : base(oauthAccessToken) {
        }

        public async Task<InvoiceResponse> GetInvoiceAsync(string invoiceId, bool includeLines = false,
            bool includeSettlements = false) {
            var includes = this.BuildIncludeParameter(includeLines, includeSettlements);
            return await this.GetAsync<InvoiceResponse>($"invoices/{invoiceId}{includes.ToQueryString()}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(string reference = null, int? year = null, int? offset = null, int? count = null, bool includeLines = false, bool includeSettlements = false) {
            // Build parameter list
            var parameters = this.BuildIncludeParameter(includeLines, includeSettlements);

            if (!string.IsNullOrWhiteSpace(reference)) {
                parameters.Add("reference", reference);
            }

            if (year.HasValue) {
                parameters.Add("year", year.Value.ToString());
            }

            return await this.GetListAsync<ListResponse<InvoiceResponse>>($"invoices", offset, count, parameters)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildIncludeParameter(bool includeLines = false,
            bool includeSettlements = false) {
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