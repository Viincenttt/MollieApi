using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record VoucherPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public VoucherPaymentResponseDetails? Details { get; set; }
    }

    public record VoucherPaymentResponseDetails : PaymentResponseDetails {
        /// <summary>
        /// The brand name of the first voucher applied.
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// A list of details of all vouchers that were used for this payment.
        /// </summary>
        public List<Voucher>? Vouchers { get; set; }

        /// <summary>
        /// Only available if another payment method was used to pay the remainder amount – The amount that remained
        /// after all vouchers were applied.
        /// </summary>
        public Amount? RemainderAmount { get; set; }

        /// <summary>
        /// Only available if another payment method was used to pay the remainder amount – The payment method that was
        /// used to pay the remainder amount.
        /// </summary>
        public string? RemainderMethod { get; set; }

        /// <summary>
        /// Optional include - The full payment method details of the remainder payment, as raw JSON.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? RemainderDetails { get; set; }

        public T? GetRemainderDetails<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
            return RemainderDetails != null ? JsonSerializer.Deserialize<T>(RemainderDetails, jsonSerializerOptions) : default;
        }
    }

    public record Voucher {
        /// <summary>
        /// The ID of the voucher brand that was used during the payment.
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// The amount that was paid with this voucher.
        /// </summary>
        public Amount? Amount { get; set; }
    }
}
