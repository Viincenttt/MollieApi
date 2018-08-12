using System.Collections.Generic;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Models.List.Specific {
    public class PaymentListData {
        public List<PaymentResponse> Payments { get; set; }
    }
}
