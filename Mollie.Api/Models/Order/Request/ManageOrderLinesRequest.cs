using System.Collections.Generic;

namespace Mollie.Api.Models.Order.Request {
    public class ManageOrderLinesRequest {
        /// <summary>
        /// List of operations to be processed.
        /// </summary>
        public IList<ManageOrderLinesOperation> Operations { get; set; }
    }

    public static class OrderLineOperation {
        public const string Add = "add";
        public const string Update = "update";
        public const string Cancel = "cancel";
    }

    public class ManageOrderLinesAddOperationData : OrderLineRequest {
    }

    public class ManageOrderLinesUpdateOperationData : OrderLineUpdateRequest {
        public string Id { get; set; } 
    }

    public class ManagerOrderLinesCancelOperationData : OrderLineDetails {
    }

    public abstract class ManageOrderLinesOperation {
        /// <summary>
        /// Operation type. Either `add`, `update`, or `cancel`.
        /// </summary>
        public string Operation { get; protected set; }
    }

    public class ManageOrderLinesAddOperation : ManageOrderLinesOperation {
        public ManageOrderLinesAddOperationData Data { get; set; }
        
        public ManageOrderLinesAddOperation() {
            this.Operation = OrderLineOperation.Add;
        }
    }

    public class ManageOrderLinesUpdateOperation : ManageOrderLinesOperation {
        public ManageOrderLinesUpdateOperationData Data { get; set; }
        
        public ManageOrderLinesUpdateOperation() {
            this.Operation = OrderLineOperation.Update;
        }
    }

    public class ManageOrderLinesCancelOperation : ManageOrderLinesOperation {
        public ManagerOrderLinesCancelOperationData Data { get; set; }
        
        public ManageOrderLinesCancelOperation() {
            this.Operation = OrderLineOperation.Cancel;
        }
    }
}