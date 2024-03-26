namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public class ManageOrderLinesUpdateOperation : ManageOrderLinesOperation {
        public required ManageOrderLinesUpdateOperationData Data { get; init; }
        
        public ManageOrderLinesUpdateOperation() {
            this.Operation = OrderLineOperation.Update;
        }
    }
}