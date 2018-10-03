using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Order {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus {
        [EnumMember(Value = "created")] Created,
        [EnumMember(Value = "paid")] Paid,
        [EnumMember(Value = "authorized")] Authorized,
        [EnumMember(Value = "canceled")] Canceled,
        [EnumMember(Value = "shipping")] Shipping,
        [EnumMember(Value = "completed")] Completed,
        [EnumMember(Value = "expired")] Expired
    }
}