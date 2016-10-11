using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mollie.Api.Models.Mandate
{
    public enum MandateStatus
    {
        [EnumMember(Value = "valid")]
        Valid,
        [EnumMember(Value = "invalid")]
        Invalid,
        [EnumMember(Value = "pending")]
        Pending
    }
}
