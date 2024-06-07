namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public record ManageOrderLinesAddOperation : ManageOrderLinesOperation {
        public required ManageOrderLinesAddOperationData Data { get; set; }

        public ManageOrderLinesAddOperation() {
            Operation = OrderLineOperation.Add;
        }
    }
}
