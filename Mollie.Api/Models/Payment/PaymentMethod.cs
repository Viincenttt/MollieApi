using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentMethod {
        [EnumMember(Value = "bancontact")] Bancontact,
        [EnumMember(Value = "banktransfer")] BankTransfer,
        [EnumMember(Value = "belfius")] Belfius,
        [EnumMember(Value = "bitcoin")] Bitcoin,
        [EnumMember(Value = "creditcard")] CreditCard,
        [EnumMember(Value = "directdebit")] DirectDebit,
        [EnumMember(Value = "eps")] Eps,
        [EnumMember(Value = "giftcard")] GiftCard,
        [EnumMember(Value = "giropay")] Giropay,
        [EnumMember(Value = "ideal")] Ideal,
        [EnumMember(Value = "inghomepay")] IngHomePay,
        [EnumMember(Value = "kbc")] Kbc,
        [EnumMember(Value = "paypal")] PayPal,
        [EnumMember(Value = "paysafecard")] PaySafeCard,
        [EnumMember(Value = "sofort")] Sofort,
        [EnumMember(Value = "refund")] Refund,
        [EnumMember(Value = "klarnapaylater")] KlarnaPayLater,
        [EnumMember(Value = "klarnasliceit")] KlarnaSliceIt,
        [EnumMember(Value = "przelewy24")] Przelewy24,
        [EnumMember(Value = "applepay")] ApplePay,
        [EnumMember(Value = "mealvoucher")] MealVoucher
    }
}