using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Organization;

public record PartnerResponse {
    /// <summary>
    /// Indicates the response contains a partner status object. Will always contain the string partner for this endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// Indicates the type of partner. Will be null if the currently authenticated organization is not enrolled as a partner.
    /// Use the Mollie.Api.Models.Organization.PartnerTypes class for a full list of known values.
    /// </summary>
    public string? PartnerType { get; set; }

    /// <summary>
    /// Whether the current organization is receiving commissions.
    /// </summary>
    public required bool IsCommissionPartner { get; set; }

    /// <summary>
    /// Array of User-Agent token objects. Present if the organization is a partner of type useragent, or if they were in the past.
    /// </summary>
    public IEnumerable<UserAgentToken> UserAgentTokens { get; set; } = new List<UserAgentToken>();

    /// <summary>
    /// The date the partner contract was signed, in ISO 8601 format. Omitted if no contract has been signed (yet).
    /// </summary>
    public DateTimeOffset? PartnerContractSignedAt { get; set; }

    /// <summary>
    /// Whether an update to the partner contract is available and requiring the organization's agreement.
    /// </summary>
    public required bool PartnerContractUpdateAvailable  { get; set; }

    /// <summary>
    /// The expiration date of the signed partner contract, in ISO 8601 format. Omitted if contract has no expiration date (yet).
    /// </summary>
    public DateTimeOffset? PartnerContractExpiresAt  { get; set; }

    /// <summary>
    /// An object with several relevant URLs. Every URL object will contain an href and a type field.
    /// </summary>
    [JsonPropertyName("_links")]
    public required PartnerResponseLinks Links { get; set; }
}

public record PartnerResponseLinks {
    /// <summary>
    /// In v2 endpoints, URLs are commonly represented as objects with an href and type field.
    /// </summary>
    public required UrlLink Self { get; set; }

    /// <summary>
    /// The URL that can be used to have new organizations sign up and be automatically linked to this partner.
    /// Will be omitted if the partner is not of type signuplink.
    /// </summary>
    public UrlLink? Signuplink { get; set; }

    /// <summary>
    /// In v2 endpoints, URLs are commonly represented as objects with an href and type field.
    /// </summary>
    public required UrlLink Documentation { get; set; }
}
