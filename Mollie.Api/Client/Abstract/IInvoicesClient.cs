using System.Threading.Tasks;
using Mollie.Api.Models.Invoice;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client.Abstract
{
	public interface IInvoicesClient
	{
		/// <summary>
		/// Retrieve details of an invoice, using the invoice's identifier.
		/// </summary>
		/// <param name="invoiceId">The invoice's ID</param>
		/// <param name="includeLines">Include invoice details such as which products were invoiced.</param>
		/// <param name="includeSettlements">Include settlements for which the invoice was created, if applicable.</param>
		/// <returns></returns>
		Task<InvoiceResponse> GetInvoiceAsync(string invoiceId, bool includeLines = false, bool includeSettlements = false);


		Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(int? offset = null, int? count = null);
	}
}