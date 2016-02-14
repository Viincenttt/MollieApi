using System.Runtime.Serialization;

namespace Mollie.Api.Models.Payment {
    /// <summary>
    /// The mode used to create this payment. Mode determines whether a payment is real or a test payment.
    /// </summary>
    public enum PaymentMode {
        [EnumMember(Value = "live")]
        Live,
        [EnumMember(Value = "test")]
        Test
    }
}
