namespace Mollie.Api.Models.Order {
    public class OrderLineDetails {
        public string Id { get; set; }
        public int? Quantity { get; set; }
        public Amount Amount { get; set; }
    }
}
