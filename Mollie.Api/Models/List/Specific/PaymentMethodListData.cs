using System.Collections.Generic;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.Api.Models.List.Specific {
    public class PaymentMethodListData {
        public List<PaymentMethodResponse> Methods { get; set; }
    }
}