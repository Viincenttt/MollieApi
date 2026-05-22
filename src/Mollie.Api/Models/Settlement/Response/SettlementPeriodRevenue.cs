namespace Mollie.Api.Models.Settlement.Response;

public record SettlementPeriodRevenue
{
	/// <summary>
	/// A description of the revenue subtotal
	/// </summary>
	public required string Description { get; set; }

    /// <summary>
    /// The net total of received funds, i.e. excluding VAT
    /// </summary>
    public required Amount AmountNet { get; set; }

    /// <summary>
    /// The applicable VAT
    /// </summary>
    public Amount? AmountVat { get; set; }

    /// <summary>
    /// The gross total of received funds, i.e. including VAT
    /// </summary>
    public required Amount AmountGross { get; set; }

	/// <summary>
	/// The number of payments
	/// </summary>
	public int Count { get; set; }

	/// <summary>
	/// The payment method, if applicable
	/// </summary>
	public string? Method { get; set; }
}

