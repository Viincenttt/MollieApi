namespace Mollie.Api.Models.Invoice
{
	public class InvoiceLinks
	{
		/// <summary>
		/// The URL to the PDF version of the invoice. The URL will expire after 60 minutes.
		/// </summary>
		public string Pdf { get; set; }
	}
}