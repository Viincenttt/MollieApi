using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Wallet.Response {
    public record ApplePayPaymentSessionResponse {
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public required DateTime EpochTimestamp { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public required DateTime ExpiresAt { get; set; }
        public required string MerchantSessionIdentifier { get; set; }
        public required string Nonce { get; set; }
        public required string MerchantIdentifier { get; set; }
        public required string DomainName { get; set; }
        public required string DisplayName { get; set; }
        public required string Signature { get; set; }
    }
}
