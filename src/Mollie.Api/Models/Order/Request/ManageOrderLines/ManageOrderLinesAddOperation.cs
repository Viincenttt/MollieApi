namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public class ManageOrderLinesAddOperation : ManageOrderLinesOperation {
        public ManageOrderLinesAddOperationData Data { get; set; }
        
        public ManageOrderLinesAddOperation() {
            this.Operation = OrderLineOperation.Add;
        }
    }
}