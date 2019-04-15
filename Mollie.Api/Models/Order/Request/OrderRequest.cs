using System;
using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Order {
    public class OrderRequest {
        /// <summary>
        /// The total amount of the order, including VAT and discounts. This is the amount that will be charged
        /// to your customer.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// The order number. For example, 16738. We recommend that each order should have a unique order number.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The lines in the order. Each line contains details such as a description of the item ordered, its
        /// price et cetera. 
        /// </summary>
        public IEnumerable<OrderLineRequest> Lines { get; set; }

        /// <summary>
        /// The billing person and address for the order.
        /// </summary>
        public OrderAddressDetails BillingAddress { get; set; }

        /// <summary>
        /// The shipping address for the order. See Order address details for the exact fields needed. If omitted,
        /// it is assumed to be identical to the billingAddress.
        /// </summary>
        public OrderAddressDetails ShippingAddress { get; set; }

        /// <summary>
        /// The date of birth of your customer. Some payment methods need this value and if you have it, you should
        /// send it so that your customer does not have to enter it again later in the checkout process.
        /// </summary>
        public DateTime? ConsumerDateOfBirth { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to after the payment process.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send order status changes to.
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// Allows you to preset the language to be used in the hosted payment pages shown to the consumer. 
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Normally, a payment method selection screen is shown. However, when using this parameter, your customer
        /// will skip the selection screen and will be sent directly to the chosen payment method. The parameter enables
        /// you to fully integrate the payment method selection into your website.
        /// </summary>
        public Payment.PaymentMethod? Method { get; set; }

        /// <summary>
        /// Provide any data you like, and we will save the data alongside the subscription. Whenever you fetch the subscription
        /// with our API, we’ll also include the metadata. You can use up to 1kB of JSON.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings jsonSerializerSettings = null) {
            this.Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}