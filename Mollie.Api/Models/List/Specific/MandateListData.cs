using System.Collections.Generic;
using Mollie.Api.Models.Mandate;

namespace Mollie.Api.Models.List.Specific {
    public class MandateListData {
        public List<MandateResponse> Mandates { get; set; }
    }
}