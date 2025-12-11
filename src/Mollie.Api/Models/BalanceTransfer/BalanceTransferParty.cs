namespace Mollie.Api.Models.BalanceTransfer;

public record BalanceTransferParty {
    /// <summary>
    /// Defines the type of the party. At the moment, only organization is supported.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Identifier of the party. For example, this contains the organization token if the type is organization.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The transfer description for the transfer party. This is the description that will appear in the financial reports of the party.
    /// </summary>
    public required string Description { get; set; }
}
