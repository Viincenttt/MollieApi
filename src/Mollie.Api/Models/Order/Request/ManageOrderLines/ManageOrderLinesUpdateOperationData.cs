namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public class ManageOrderLinesUpdateOperationData : OrderLineUpdateRequest {
        public required string Id { get; init; } 
    }
}