using System.Linq;
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

        [Test]
        public async Task CanRetrieveIdealIssuers() {
            // When: retrieving the ideal method we can include the issuers
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal, true);

            // Then: We should have one or multiple issuers
            Assert.IsNotNull(paymentMethod);
            Assert.IsTrue(paymentMethod.Issuers.Any());
        }

        [Test]
        public async Task DoNotRetrieveIssuersWhenIncludeIsFalse() {
            // When: retrieving the ideal method with the include parameter set to false
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal, false);

            // Then: Issuers should not be included
            Assert.IsNull(paymentMethod.Issuers);
        }

        [Test]
        public async Task DoNotRetrieveIssuersWhenIncludeIsNull() {
            // When: retrieving the ideal method with the include parameter set to null
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal, null);

            // Then: Issuers should not be included
            Assert.IsNull(paymentMethod.Issuers);
        }

        [Test]
        public async Task CanRetrievePricing() {
            // When: retrieving the ideal method we can include the issuers
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal, includePricing: true);

            // Then: We should have one or multiple issuers
            Assert.IsNotNull(paymentMethod);
            Assert.IsTrue(paymentMethod.Pricing.Any());
        }

        [Test]
        public async Task DoNotRetrievePricingWhenIncludeIsFalse() {
            // When: retrieving the ideal method with the include parameter set to false
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal, includePricing: false);

            // Then: Issuers should not be included
            Assert.IsNull(paymentMethod.Pricing);
        }

        [Test]
        public async Task DoNotRetrievePricingWhenIncludeIsNull() {
            // When: retrieving the ideal method with the include parameter set to null
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal, includePricing: null);

            // Then: Issuers should not be included
            Assert.IsNull(paymentMethod.Pricing);
        }

        [Test]
        public async Task CanRetrieveAllMethods() {
            // When: retrieving the all mollie payment methods
            ListResponse<PaymentMethodResponse> paymentMethods = await this._paymentMethodClient.GetAllPaymentMethodListAsync();

            // Then: We should have multiple issuers
            Assert.IsNotNull(paymentMethods);
            Assert.IsTrue(paymentMethods.Items.Any());
        }

        [Test]
        public async Task CanRetrievePricingForAllMethods() {
            // When: retrieving the ideal method we can include the issuers
            ListResponse<PaymentMethodResponse> paymentMethods = await this._paymentMethodClient.GetAllPaymentMethodListAsync(includePricing: true);

            // Then: We should have prices available
            Assert.IsTrue(paymentMethods.Items.Any(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0)));
        }

        [Test]
        public async Task CanRetrieveIssuersForAllMethods() {
            // When: retrieving the all mollie payment methods we can include the issuers
            ListResponse<PaymentMethodResponse> paymentMethods = await this._paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true);

            // Then: We should have one or multiple issuers
            Assert.IsTrue(paymentMethods.Items.Any(x => x.Issuers != null));
        }
    }
}