using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Wallet.Response {
    public class ApplePayPaymentSessionResponse {
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime EpochTimestamp { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime ExpiresAt { get; set; }
        public string MerchantSessionIdentifier { get; set; }
        public string Nonce { get; set; }
        public string MerchantIdentifier { get; set; }
        public string DomainName { get; set; }
        public string DisplayName { get; set; }
        public string Signature { get; set; }
    }
}