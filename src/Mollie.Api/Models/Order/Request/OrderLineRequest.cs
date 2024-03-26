using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Order {
    public class OrderLineRequest {
        /// <summary>
        /// The type of product bought, for example, a physical or a digital product. See the 
        /// Mollie.Api.Models.Order.OrderLineDetailsType class for a full list of known values.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// The category of product bought. See the Mollie.Api.Models.Order.OrderLineDetailsCategory class
        /// for a full list of known values
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// A description of the order line, for example LEGO 4440 Forest Police Station.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// The number of items in the order line.
        /// </summary>
        public required int Quantity { get; init; }

        /// <summary>
        /// The price of a single item in the order line.
        /// </summary>
        public required Amount UnitPrice { get; init; }

        /// <summary>
        /// Any discounts applied to the order line. For example, if you have a two-for-one sale, you should pass the
        /// amount discounted as a positive amount.
        /// </summary>
        public Amount? DiscountAmount { get; set; }

        /// <summary>
        /// The total amount of the line, including VAT and discounts. Adding all totalAmount values together should
        /// result in the same amount as the amount top level property.
        /// </summary>
        public required Amount TotalAmount { get; init; }

        /// <summary>
        /// The VAT rate applied to the order line, for example "21.00" for 21%. The vatRate should be passed as a
        /// string and not as a float to ensure the correct number of decimals are passed.
        /// </summary>
        public required string VatRate { get; init; }

        /// <summary>
        /// The amount of value-added tax on the line. The totalAmount field includes VAT, so the vatAmount can be
        /// calculated with the formula totalAmount × (vatRate / (100 + vatRate)). Any deviations from this will
        /// result in an error.
        /// </summary>
        public required Amount VatAmount { get; init; }

        /// <summary>
        /// The SKU, EAN, ISBN or UPC of the product sold. The maximum character length is 64.
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// A link pointing to an image of the product sold.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// A link pointing to the product page in your web shop of the product sold.
        /// </summary>
        public string? ProductUrl { get; set; }

        /// <summary>
        /// The optional metadata you provided upon line creation.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }
    }
}