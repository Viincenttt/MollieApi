using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Order {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderLineDetailsType {
        [EnumMember(Value = "physical")] Physical,
        [EnumMember(Value = "discount")] Discount,
        [EnumMember(Value = "digital")] Digital,
        [EnumMember(Value = "shipping_fee")] ShippingFee,
        [EnumMember(Value = "store_credit")] StoreCredit,
        [EnumMember(Value = "gift_card")] GiftCard,
        [EnumMember(Value = "surcharge")] Surcharge,
    }
}