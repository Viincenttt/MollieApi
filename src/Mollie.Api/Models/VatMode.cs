namespace Mollie.Api.Models;

/// <summary>
/// The VAT mode to use for VAT calculation. 
/// </summary>
public static class VatMode {
    /// <summary>
    /// Inclusive means the prices you are providing to us already contain the VAT you want to apply.
    /// </summary>
    public const string Inclusive = "inclusive";
    
    /// <summary>
    /// Exclusive mode means we will apply the relevant VAT on top of the price. 
    /// </summary>
    public const string Exclusive = "exclusive";
}
