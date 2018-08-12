using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SequenceType {
        [EnumMember(Value = "oneoff")] OneOff,
        [EnumMember(Value = "first")] First,
        [EnumMember(Value = "recurring")] Recurring
    }
}