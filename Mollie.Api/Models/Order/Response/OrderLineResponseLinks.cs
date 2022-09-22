using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Order {
    public class OrderLineResponseLinks {
        /// <summary>
        /// A link pointing to the product page in your web shop of the product sold.
        /// </summary>
        public UrlLink ProductUrl { get; set; }
        
        /// <summary>
        /// A link pointing to an image of the product sold.
        /// </summary>
        public UrlLink ImageUrl { get; set; }
    }
}