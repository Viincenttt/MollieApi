using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mollie.Api.Models.Payment
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RecurringType
    {
        [EnumMember(Value = "first")]
        First,
        [EnumMember(Value = "recurring")]
        Recurring
    }
}
