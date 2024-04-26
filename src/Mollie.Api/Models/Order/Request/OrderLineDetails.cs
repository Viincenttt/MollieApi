namespace Mollie.Api.Models.Order {
    public record OrderLineDetails {
        public required string Id { get; init; }
        public int? Quantity { get; set; }
        public Amount? Amount { get; set; }
    }
}
