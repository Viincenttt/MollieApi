﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Order.Request.PaymentSpecificParameters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Order.Request {
    public record OrderRequest {
        /// <summary>
        /// The total amount of the order, including VAT and discounts. This is the amount that will be charged
        /// to your customer.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The order number. For example, 16738. We recommend that each order should have a unique order number.
        /// </summary>
        public required string OrderNumber { get; set; }

        /// <summary>
        /// The lines in the order. Each line contains details such as a description of the item ordered, its
        /// price et cetera.
        /// </summary>
        public required IEnumerable<OrderLineRequest> Lines { get; set; }

        /// <summary>
        /// The billing person and address for the order.
        /// </summary>
        public OrderAddressDetails? BillingAddress { get; set; }

        /// <summary>
        /// The shipping address for the order. See Order address details for the exact fields needed. If omitted,
        /// it is assumed to be identical to the billingAddress.
        /// </summary>
        public OrderAddressDetails? ShippingAddress { get; set; }

        /// <summary>
        /// The date of birth of your customer. Some payment methods need this value and if you have it, you should
        /// send it so that your customer does not have to enter it again later in the checkout process.
        /// </summary>
        public DateTime? ConsumerDateOfBirth { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to after the payment process.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The URL your consumer will be redirected to when the consumer explicitly cancels the payment. If this URL
        /// is not provided, the consumer will be redirected to the redirectUrl instead — see above.
        ///
        /// Mollie will always give you status updates via webhooks, including for the canceled status. This parameter
        /// is therefore entirely optional, but can be useful when implementing a dedicated consumer-facing flow to handle
        /// payment cancellations.
        ///
        /// The parameter can be omitted for orders with payment.sequenceType set to recurring.
        /// </summary>
        public string? CancelUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send order status changes to.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Allows you to preset the language to be used in the hosted payment pages shown to the consumer.
        /// </summary>
        public required string Locale { get; set; }

        /// <summary>
        /// Normally, a payment method selection screen is shown. However, when using this parameter, your customer
        /// will skip the selection screen and will be sent directly to the chosen payment method. The parameter enables
        /// you to fully integrate the payment method selection into your website. See the
        /// Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
        /// </summary>
        [JsonIgnore]
        public string? Method {
            get => Methods?.FirstOrDefault();
            set {
                if (value == null) {
                    Methods = null;
                }
                else {
                    Methods = new List<string>();
                    Methods.Add(value);
                }
            }
        }

        /// <summary>
        /// Normally, a payment method screen is shown. However, when using this parameter, you can choose a specific payment method
        /// and your customer will skip the selection screen and is sent directly to the chosen payment method. The parameter
        /// enables you to fully integrate the payment method selection into your website.
        /// You can also specify the methods in an array.By doing so we will still show the payment method selection screen but will
        /// only show the methods specified in the array. For example, you can use this functionality to only show payment methods
        /// from a specific country to your customer.
        /// </summary>
        [JsonProperty("method")]
        public IList<string>? Methods { get; set; }

        /// <summary>
        /// Optional - Any payment specific properties can be passed here.
        /// </summary>
        public OrderPaymentParameters? Payment { get; set; }

        /// <summary>
        /// Provide any data you like, and we will save the data alongside the subscription. Whenever you fetch the subscription
        /// with our API, we’ll also include the metadata. You can use up to 1kB of JSON.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// The date the order should expire in YYYY-MM-DD format. The minimum date is tomorrow and the maximum date is 100 days
        /// after tomorrow.
        /// </summary>
        public string? ExpiresAt { get; set; }

        /// <summary>
        /// For digital goods, you must make sure to apply the VAT rate from your customer’s country in most jurisdictions. Use
        /// this parameter to restrict the payment methods available to your customer to methods from the billing country only.
        /// </summary>
        public bool? ShopperCountryMustMatchBillingCountry { get; set; }

        /// <summary>
        ///	Oauth only - The payment profile's unique identifier, for example pfl_3RkSN1zuPE.
        /// </summary>
        public string? ProfileId { get; set; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this payment a test payment.
        /// </summary>
        public bool? Testmode { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}
