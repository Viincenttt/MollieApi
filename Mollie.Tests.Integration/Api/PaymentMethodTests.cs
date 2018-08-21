using System.Threading.Tasks;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class PaymentMethodTests : BaseMollieApiTestClass {
        [Test]
        public async Task CanRetrievePaymentMethodList() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentMethodResponse> response = await this._paymentMethodClient.GetPaymentMethodListAsync();

            // Then: Make sure it can be retrieved
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }

        [TestCase(PaymentMethod.Ideal)]
        [TestCase(PaymentMethod.CreditCard)]
        [TestCase(PaymentMethod.Bancontact)]
        [TestCase(PaymentMethod.Sofort)]
        [TestCase(PaymentMethod.BankTransfer)]
        [TestCase(PaymentMethod.Belfius)]
        [TestCase(PaymentMethod.Bitcoin)]
        [TestCase(PaymentMethod.PayPal)]
        [TestCase(PaymentMethod.Kbc)]
        public async Task CanRetrieveSinglePaymentMethod(PaymentMethod method) {
            // When: retrieving a payment method
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(method);

            // Then: Make sure it can be retrieved
            Assert.IsNotNull(paymentMethod);
            Assert.IsNotNull(paymentMethod.Id);
            Assert.AreEqual(method, paymentMethod.Id);
        }
    }
}