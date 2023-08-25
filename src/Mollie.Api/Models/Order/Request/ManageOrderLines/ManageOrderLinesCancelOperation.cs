namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public class ManageOrderLinesCancelOperation : ManageOrderLinesOperation {
        public ManagerOrderLinesCancelOperationData Data { get; set; }
        
        public ManageOrderLinesCancelOperation() {
            this.Operation = OrderLineOperation.Cancel;
        }
    }
}