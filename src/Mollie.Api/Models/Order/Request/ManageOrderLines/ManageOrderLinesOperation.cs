namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    public abstract record ManageOrderLinesOperation {
        /// <summary>
        /// Operation type. Either `add`, `update`, or `cancel`.
        /// </summary>
        public string Operation { get; protected set; } = string.Empty;
    }
}