namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public record ManageOrderLinesUpdateOperation : ManageOrderLinesOperation {
        public required ManageOrderLinesUpdateOperationData Data { get; init; }
        
        public ManageOrderLinesUpdateOperation() {
            this.Operation = OrderLineOperation.Update;
        }
    }
}