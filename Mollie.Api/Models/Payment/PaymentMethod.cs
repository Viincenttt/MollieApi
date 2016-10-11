using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentMethod {
        [EnumMember(Value = "ideal")]
        Ideal,
        [EnumMember(Value = "creditcard")]
        CreditCard,
        [EnumMember(Value = "mistercash")]
        MisterCash,
        [EnumMember(Value = "sofort")]
        Sofort,
        [EnumMember(Value = "banktransfer")]
        BankTransfer,
        [EnumMember(Value = "directdebit")]
        DirectDebit,
        [EnumMember(Value = "belfius")]
        Belfius,
        [EnumMember(Value = "bitcoin")]
        Bitcoin,
        [EnumMember(Value = "podiumcadeaukaart")]
        PodiumCadeaukaart,
        [EnumMember(Value = "paypal")]
        PayPal,
        [EnumMember(Value = "paysafecard")]
        PaySafeCard,
        [EnumMember(Value = "kbc")]
        Kbc,
    }
}
