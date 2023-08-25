namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public class ManageOrderLinesUpdateOperation : ManageOrderLinesOperation {
        public ManageOrderLinesUpdateOperationData Data { get; set; }
        
        public ManageOrderLinesUpdateOperation() {
            this.Operation = OrderLineOperation.Update;
        }
    }
}