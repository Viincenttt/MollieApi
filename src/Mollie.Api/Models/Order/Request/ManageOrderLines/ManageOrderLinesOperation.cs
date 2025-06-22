using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Order.Request.ManageOrderLines {
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "operation")]
    [JsonDerivedType(typeof(ManageOrderLinesAddOperation), OrderLineOperation.Add)]
    [JsonDerivedType(typeof(ManageOrderLinesCancelOperation), OrderLineOperation.Cancel)]
    [JsonDerivedType(typeof(ManageOrderLinesUpdateOperation), OrderLineOperation.Update)]
    public abstract record ManageOrderLinesOperation {
        /// <summary>
        /// Operation type. Either `add`, `update`, or `cancel`.
        /// </summary>
        public string Operation { get; protected set; } = string.Empty;
    }
}
