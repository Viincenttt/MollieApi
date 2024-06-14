﻿using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Refund.Request {
    public record RefundRequest {
        /// <summary>
        /// The amount to refund. For some payments, it can be up to €25.00 more than the original transaction amount.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// Optional – The description of the refund you are creating. This will be shown to the consumer on their card or bank
        /// statement when possible. Max 140 characters.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the refund. Whenever
        /// you fetch the refund with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// Set this to true to refund a test mode payment.
        /// </summary>
        public bool? Testmode { get; set; }

        /// <summary>
        /// If you wish to pull back the money that was sent to connected accounts within the creation of a partial
        /// refund (namely a refund of less of the amount of the original payment), you can do so by setting the
        /// routingReversals array in the request
        /// </summary>
        public IList<RoutingReversal>? RoutingReversals { get; set; }

        /// <summary>
        /// For a full reversal of the split that was specified during payment creation, simply set reverseRouting=true
        /// when creating the refund, so that a full compensation is created for every route of the original payment.
        /// This flag only works with full refunds, namely a refund of the same amount (or more) than the original payment.
        /// </summary>
        public bool? ReverseRouting { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}
