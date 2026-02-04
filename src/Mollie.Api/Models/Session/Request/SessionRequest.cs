using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Session.Request {
    public record SessionRequest : ITestModeRequest, IProfileRequest {
        /// <summary>
        /// The amount that you want to charge, e.g. {currency:"EUR", value:"1000.00"} if you would want to charge €1000.00.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The description of the session.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Required - The URL the consumer will be redirected to after the payment process. It could make sense for the
        /// redirectURL to contain a unique
        /// identifier – like your order ID – so you can show the right page referencing the order when the consumer returns.
        /// </summary>
        public required string RedirectUrl { get; set; }

        /// <summary>
        /// The URL your consumer will be redirected to when the consumer explicitly cancels the payment. If this URL is not
        /// provided, the consumer will be redirected to the redirectUrl instead — see above.

        /// Mollie will always give you status updates via webhooks, including for the canceled status. This parameter is
        /// therefore entirely optional, but can be useful when implementing a dedicated consumer-facing flow to handle payment
        /// cancellations.
        /// </summary>
        public string? CancelUrl { get; set; }

        /// <summary>
        /// Optionally provide the order lines for the payment. Each line contains details such as a description of the item ordered and its price.
        /// All lines must have the same currency as the payment.
        /// </summary>
        public List<PaymentLine>? Lines { get; set; }

        /// <summary>
        /// The customer's billing address details. We advise to provide these details to improve fraud protection and conversion. This is
        /// particularly relevant for card payments.
        /// </summary>
        public PaymentAddressDetails? BillingAddress { get; set; }

        /// <summary>
        /// The customer's shipping address details. We advise to provide these details to improve fraud protection and conversion. This is
        /// particularly relevant for card payments.
        /// </summary>
        public PaymentAddressDetails? ShippingAddress { get; set; }

        public SessionPaymentRequest? Payment { get; set; }

        /// <summary>
        /// The ID of the Customer for whom the payment is being created. This is used for recurring payments and single click payments.
        /// </summary>
        public string? CustomerId { get; set; }

        /// <summary>
        /// Indicate which type of payment this is in a recurring sequence. If set to first, a first payment is created for the
        /// customer, allowing the customer to agree to automatic recurring charges taking place on their account in the future.
        /// If set to recurring, the customer’s card is charged automatically. Defaults to oneoff, which is a regular non-recurring
        /// payment(see also: Recurring). See the Mollie.Api.Models.Payment.SequenceType class for a full list of known values.
        /// </summary>
        public string? SequenceType { get; set; }

        /// <summary>
        /// The website profile’s unique identifier, for example pfl_3RkSN1zuPE.
        /// </summary>
        public string? ProfileId { get; set; }

        /// <summary>
        /// Provide any data you like, and we will save the data alongside the session. Whenever you fetch the session
        /// with our API, we’ll also include the metadata. You can use up to 1kB of JSON.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this session a test session.
        /// </summary>
        public bool? Testmode { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerOptions? jsonSerializerOptions = null) {
            Metadata = JsonSerializer.Serialize(metadataObj, jsonSerializerOptions);
        }
    }
}
