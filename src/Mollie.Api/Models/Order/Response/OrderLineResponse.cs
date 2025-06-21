using System;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Order.Response {
    public record OrderLineResponse {
        /// <summary>
        /// The order line’s unique identifier, for example odl_dgtxyl.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The ID of the order the line belongs too, for example ord_kEn1PlbGa.
        /// </summary>
        public required string OrderId { get; set; }

        /// <summary>
        /// The type of product bought, for example, a physical or a digital product. See the
        /// Mollie.Api.Models.Order.OrderLineDetailsType class for a full list of known values.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// A description of the order line, for example LEGO 4440 Forest Police Station.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Status of the order line - See the Mollie.Api.Models.Order.OrderLineStatus class for
        /// a full list of known values.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// Whether or not the order line can be (partially) canceled.
        /// </summary>
        public required bool IsCancelable { get; set; }

        /// <summary>
        /// The number of items in the order line.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// The number of items that are shipped for this order line.
        /// </summary>
        public required int QuantityShipped { get; set; }

        /// <summary>
        /// The total amount that is shipped for this order line.
        /// </summary>
        public required Amount AmountShipped { get; set; }

        /// <summary>
        /// The number of items that are refunded for this order line.
        /// </summary>
        public required int QuantityRefunded { get; set; }

        /// <summary>
        /// The total amount that is refunded for this order line.
        /// </summary>
        public required Amount AmountRefunded { get; set; }

        /// <summary>
        /// The number of items that are canceled in this order line.
        /// </summary>
        public required int QuantityCanceled { get; set; }

        /// <summary>
        /// The total amount that is canceled in this order line.
        /// </summary>
        public required Amount AmountCanceled { get; set; }

        /// <summary>
        /// The number of items that can still be shipped for this order line.
        /// </summary>
        public required int ShippableQuantity { get; set; }

        /// <summary>
        /// The number of items that can still be refunded for this order line.
        /// </summary>
        public required int RefundableQuantity { get; set; }

        /// <summary>
        /// The number of items that can still be canceled for this order line.
        /// </summary>
        public required int CancelableQuantity { get; set; }

        /// <summary>
        /// The price of a single item in the order line.
        /// </summary>
        public required Amount UnitPrice { get; set; }

        /// <summary>
        /// Any discounts applied to the order line. For example, if you have a two-for-one sale, you should pass the
        /// amount discounted as a positive amount.
        /// </summary>
        public Amount? DiscountAmount { get; set; }

        /// <summary>
        /// The total amount of the line, including VAT and discounts. Adding all totalAmount values together should
        /// result in the same amount as the amount top level property.
        /// </summary>
        public required Amount TotalAmount { get; set; }

        /// <summary>
        /// The VAT rate applied to the order line, for example "21.00" for 21%. The vatRate should be passed as a
        /// string and not as a float to ensure the correct number of decimals are passed.
        /// </summary>
        public required string VatRate { get; set; }

        /// <summary>
        /// The amount of value-added tax on the line. The totalAmount field includes VAT, so the vatAmount can be
        /// calculated with the formula totalAmount × (vatRate / (100 + vatRate)). Any deviations from this will
        /// result in an error.
        /// </summary>
        public required Amount VatAmount { get; set; }

        /// <summary>
        /// The SKU, EAN, ISBN or UPC of the product sold. The maximum character length is 64.
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// The order line’s date and time of creation
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order line. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required OrderLineResponseLinks Links { get; set; }

        /// <summary>
        /// The optional metadata you provided upon line creation.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }
    }
}
