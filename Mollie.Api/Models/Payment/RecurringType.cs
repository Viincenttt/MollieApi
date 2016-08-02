using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Mollie.Api.Models.Payment {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RecurringType {
        [EnumMember(Value = "first")]
        First,
        [EnumMember(Value = "recurring")]
        Recurring
    }
}
