namespace Mollie.Api.Models.Invoice
{
	public class InvoiceAmount
	{
		/// <summary>
		/// Total amount of the invoice excluding VAT.
		/// </summary>
		public decimal Net { get; set; }

		/// <summary>
		/// Only for merchants registered in the Netherlands. For EU merchants, VAT will be shifted to recipient; article 44 and 196 EU VAT Directive 2006/112. For merchants outside the EU, no VAT will be charged. – VAT amount of the invoice.
		/// </summary>
		public decimal Vat { get; set; }

		/// <summary>
		/// Total amount of the invoice including VAT.
		/// </summary>
		public decimal Gross { get; set; }
	}
}