using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Mollie.Api.Models.Order {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderLineStatus {
        [EnumMember(Value = "created")] Created,
        [EnumMember(Value = "paid")] Paid,
        [EnumMember(Value = "authorized")] Authorized,
        [EnumMember(Value = "canceled")] Canceled,
        [EnumMember(Value = "shipping")] Shipping,
        [EnumMember(Value = "completed")] Completed,
    }
}
