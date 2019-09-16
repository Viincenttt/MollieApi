using System;
using System.Collections.Generic;
using Mollie.Api.Extensions;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Subscription;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Order {

    public class OrderEmbeddedResponse : IResponseObject {

        public IEnumerable<PaymentResponse> Payments { get; set; }

        public IEnumerable<RefundResponse> Refunds { get; set; }

    }
}