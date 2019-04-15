namespace Mollie.Api.Models.Order {
    public class OrderLineUpdateRequest {
        /// <summary>
        /// A description of the order line, for example LEGO 4440 Forest Police Station.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A link pointing to an image of the product sold.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// A link pointing to the product page in your web shop of the product sold.
        /// </summary>
        public string ProductUrl { get; set; }

        /// <summary>
        /// The number of items in the order line.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// The price of a single item including VAT in the order line.
        /// </summary>
        public Amount UnitPrice { get; set; }

        /// <summary>
        /// Any discounts applied to the order line. For example, if you have a two-for-one sale, you should pass the 
        /// amount discounted as a positive amount.
        /// </summary>
        public Amount DiscountPrice { get; set; }

        /// <summary>
        /// The total amount of the line, including VAT and discounts. Adding all totalAmount values together should 
        /// result in the same amount as the amount top level property.
        /// </summary>
        public Amount TotalAmount { get; set; }

        /// <summary>
        /// The amount of value-added tax on the line. The totalAmount field includes VAT, so the vatAmount can be 
        /// calculated with the formula totalAmount × (vatRate / (100 + vatRate)).
        /// </summary>
        public Amount VatAmount { get; set; }

        /// <summary>
        /// The VAT rate applied to the order line, for example "21.00" for 21%. The vatRate should be passed as a 
        /// string and not as a float to ensure the correct number of decimals are passed.
        /// </summary>
        public string VatRate { get; set; }
    }
}
