namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public class ManageOrderLinesCancelOperation : ManageOrderLinesOperation {
        public required ManagerOrderLinesCancelOperationData Data { get; init; }
        
        public ManageOrderLinesCancelOperation() {
            Operation = OrderLineOperation.Cancel;
        }
    }
}