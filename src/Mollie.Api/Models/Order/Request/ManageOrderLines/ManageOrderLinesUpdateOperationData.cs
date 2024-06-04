namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public record ManageOrderLinesUpdateOperationData : OrderLineUpdateRequest {
        public required string Id { get; set; }
    }
}
