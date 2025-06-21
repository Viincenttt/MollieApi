using System.Runtime.Serialization;

namespace Mollie.Api.Models {
    public enum Mode {
        [EnumMember(Value = "live")] Live,
        [EnumMember(Value = "test")] Test
    }
}
