using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Invoice;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client
{
	public class InvoicesClient : OauthBaseMollieClient, IInvoicesClient
	{
		public InvoicesClient(string oauthAccessToken) : base(oauthAccessToken) { }

		public async Task<InvoiceResponse> GetInvoiceAsync(string invoiceId, bool includeLines = false, bool includeSettlements = false)
		{
			var includes = BuildIncludeParameter(includeLines, includeSettlements);
			return await this.GetAsync<InvoiceResponse>($"invoices/{invoiceId}{includes}").ConfigureAwait(false);
		}

		public Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(string reference = null, int? year = null, int? offset = null, int? count = null,
			bool includeLines = false, bool includeSettlements = false)
		{
			throw new System.NotImplementedException();
		}

		private string BuildIncludeParameter(bool includeLines = false, bool includeSettlements = false)
		{
			var result = string.Empty;

			var includeList = new List<string>();
			if (includeLines) includeList.Add("lines");
			if (includeSettlements) includeList.Add("settlements");

			if (includeList.Any())
				result = $"?includes={string.Join(",", includeList)}";

			return result;
		}
	}
}