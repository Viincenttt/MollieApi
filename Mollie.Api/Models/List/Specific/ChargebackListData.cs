using Mollie.Api.Models.Chargeback;
using System.Collections.Generic;

namespace Mollie.Api.Models.List.Specific {
    public class ChargebackListData {
        public List<ChargebackResponse> Chargebacks { get; set; }
    }
}