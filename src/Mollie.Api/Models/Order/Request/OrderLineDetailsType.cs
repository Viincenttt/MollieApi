namespace Mollie.Api.Models.Order.Request {
    public static class OrderLineDetailsType {
        public const string Physical = "physical";
        public const string Discount = "discount";
        public const string Digital = "digital";
        public const string ShippingFee = "shipping_fee";
        public const string StoreCredit = "gift_card";
        public const string GiftCard = "gift_card";
        public const string Surcharge = "surcharge";
    }
}
