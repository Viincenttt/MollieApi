namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public record ManageOrderLinesAddOperation : ManageOrderLinesOperation {
        public required ManageOrderLinesAddOperationData Data { get; init; }
        
        public ManageOrderLinesAddOperation() {
            Operation = OrderLineOperation.Add;
        }
    }
}