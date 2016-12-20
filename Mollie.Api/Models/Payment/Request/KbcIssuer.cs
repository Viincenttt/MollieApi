namespace Mollie.Api.Models.Payment.Request {
    using System.Runtime.Serialization;

    public enum KbcIssuer {
        [EnumMember(Value = "kbc")]
        Kbc,
        [EnumMember(Value = "cbc")]
        Cbc
    }
}
