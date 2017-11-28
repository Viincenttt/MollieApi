using System.Threading.Tasks;
using Mollie.Api.Models.Invoice;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client.Abstract {
    public interface IInvoicesClient {
        /// <summary>
        ///     Retrieve details of an invoice, using the invoice's identifier.
        /// </summary>
        /// <param name="invoiceId">The invoice's ID</param>
        /// <param name="includeLines">Include invoice details such as which products were invoiced.</param>
        /// <param name="includeSettlements">Include settlements for which the invoice was created, if applicable.</param>
        /// <returns></returns>
        Task<InvoiceResponse> GetInvoiceAsync(string invoiceId, bool includeLines = false,
            bool includeSettlements = false);

        /// <summary>
        ///     Retrieve all invoices on the account. Optionally filter on year or invoice number.
        /// </summary>
        /// <param name="reference">
        ///     Optional – Use this parameter to filter for an invoice with a specific invoice number /
        ///     reference. An example reference would be 2016.10000.
        /// </param>
        /// <param name="year">Optional – Use this parameter to filter for invoices from a specific year (e.g. 2016).</param>
        /// <param name="offset">Optional – The number of objects to skip.</param>
        /// <param name="count">Optional – The number of objects to return (with a maximum of 250).</param>
        /// <param name="includeLines">Include invoice details such as which products were invoiced.</param>
        /// <param name="includeSettlements">Include settlements for which the invoice was created, if applicable.</param>
        /// <returns>List of Invoices</returns>
        Task<ListResponse<InvoiceResponse>> GetInvoiceListAsync(string reference = null, int? year = null,
            int? offset = null, int? count = null, bool includeLines = false, bool includeSettlements = false);
    }
}