namespace Mollie.Api.Models.Order.Request {
    public record OrderLineDetails {
        public required string Id { get; set; }
        public int? Quantity { get; set; }
        public Amount? Amount { get; set; }
    }
}
