using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Wallet.Response {
    public record ApplePayPaymentSessionResponse {
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public required DateTime EpochTimestamp { get; init; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public required DateTime ExpiresAt { get; init; }
        public required string MerchantSessionIdentifier { get; init; }
        public required string Nonce { get; init; }
        public required string MerchantIdentifier { get; init; }
        public required string DomainName { get; init; }
        public required string DisplayName { get; init; }
        public required string Signature { get; init; }
    }
}