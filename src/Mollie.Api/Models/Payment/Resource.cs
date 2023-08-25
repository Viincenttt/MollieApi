using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Resource
    {
        [EnumMember(Value = "orders")] Orders,
        [EnumMember(Value = "payments")] Payments
    }
}
