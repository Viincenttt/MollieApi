using System.Runtime.Serialization;

namespace Mollie.Api.Models
{
    public enum SortDirection
    {
        [EnumMember(Value = "desc")] Desc,
        [EnumMember(Value = "asc")] Asc
    }
}
