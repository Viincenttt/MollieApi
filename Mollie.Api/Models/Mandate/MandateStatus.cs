using System.Runtime.Serialization;

namespace Mollie.Api.Models.Mandate {
    public enum MandateStatus {
        [EnumMember(Value = "valid")] Valid,
        [EnumMember(Value = "invalid")] Invalid,
        [EnumMember(Value = "pending")] Pending
    }
}