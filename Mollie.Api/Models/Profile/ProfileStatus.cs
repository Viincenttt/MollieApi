using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Profile {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProfileStatus {
        [EnumMember(Value = "unverified")] Unverified,
        [EnumMember(Value = "verified")] Verified,
        [EnumMember(Value = "blocked")] Blocked
    }
}