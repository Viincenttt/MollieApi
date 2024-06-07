namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public record ManageOrderLinesUpdateOperation : ManageOrderLinesOperation {
        public required ManageOrderLinesUpdateOperationData Data { get; set; }

        public ManageOrderLinesUpdateOperation() {
            Operation = OrderLineOperation.Update;
        }
    }
}
