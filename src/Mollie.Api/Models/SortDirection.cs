using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortDirection
    {
        [EnumMember(Value = "desc")] Desc,
        [EnumMember(Value = "asc")] Asc
    }
}
