using System.Runtime.Serialization;

namespace Mollie.Api.Models.Payment
{
    public enum Resource
    {
        [EnumMember(Value = "orders")] Orders,
        [EnumMember(Value = "payments")] Payments
    }
}
