using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models {
    public enum Mode {
        [EnumMember(Value = "live")] Live,
        [EnumMember(Value = "test")] Test
    }
}
