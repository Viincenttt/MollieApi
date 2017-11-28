using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Mode {
        [EnumMember(Value = "live")] Live,
        [EnumMember(Value = "test")] Test
    }
}