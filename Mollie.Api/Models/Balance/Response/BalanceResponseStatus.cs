using System.Runtime.Serialization;

namespace Mollie.Api.Models.Balance.Response {
    public enum BalanceResponseStatus {
        [EnumMember(Value = "active")] Active,
        [EnumMember(Value = "inactive")] Inactive
    }
}