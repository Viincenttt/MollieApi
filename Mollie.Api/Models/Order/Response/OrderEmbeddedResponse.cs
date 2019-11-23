using System.Collections.Generic;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;

namespace Mollie.Api.Models.Order.Response {

    public class OrderEmbeddedResponse : IResponseObject {

        public IEnumerable<PaymentResponse> Payments { get; set; }

        public IEnumerable<RefundResponse> Refunds { get; set; }
    }
}