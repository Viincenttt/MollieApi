using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class PaymentMethodTests : BaseMollieApiTestClass {

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrievePaymentMethodList() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentMethodResponse> response = await this._paymentMethodClient.GetPaymentMethodListAsync();

            // Then: Make sure it can be retrieved
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }
        
        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrievePaymentMethodListIncludeWallets() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentMethodResponse> response = await this._paymentMethodClient.GetPaymentMethodListAsync(includeWallets: "applepay");

            // Then: Make sure it can be retrieved
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }

        [TestCase(PaymentMethod.Bancontact)]
        [TestCase(PaymentMethod.Sofort)]
        [TestCase(PaymentMethod.BankTransfer)]
        [TestCase(PaymentMethod.Belfius)]
        [TestCase(PaymentMethod.PayPal)]
        [TestCase(PaymentMethod.Kbc)]
        public async Task CanRetrieveSinglePaymentMethod(string method) {
            // When: retrieving a payment method
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(method);

            // Then: Make sure it can be retrieved
            Assert.IsNotNull(paymentMethod);
            Assert.IsNotNull(paymentMethod.Id);
            Assert.AreEqual(method, paymentMethod.Id);
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrieveKbcIssuers() {
            // When: retrieving the ideal method we can include the issuers
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc, true);

            // Then: We should have one or multiple issuers
            Assert.IsNotNull(paymentMethod);
            Assert.IsTrue(paymentMethod.Issuers.Any());
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task DoNotRetrieveIssuersWhenIncludeIsFalse() {
            // When: retrieving the ideal method with the include parameter set to false
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc, false);

            // Then: Issuers should not be included
            Assert.IsNull(paymentMethod.Issuers);
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrievePricing() {
            // When: retrieving the ideal method we can include the issuers
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.CreditCard, includePricing: true);

            // Then: We should have one or multiple issuers
            Assert.IsNotNull(paymentMethod);
            Assert.IsTrue(paymentMethod.Pricing.Any());
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task DoNotRetrievePricingWhenIncludeIsFalse() {
            // When: retrieving the ideal method with the include parameter set to false
            PaymentMethodResponse paymentMethod = await this._paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.CreditCard, includePricing: false);

            // Then: Issuers should not be included
            Assert.IsNull(paymentMethod.Pricing);
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrieveAllMethods() {
            // When: retrieving the all mollie payment methods
            ListResponse<PaymentMethodResponse> paymentMethods = await this._paymentMethodClient.GetAllPaymentMethodListAsync();

            // Then: We should have multiple issuers
            Assert.IsNotNull(paymentMethods);
            Assert.IsTrue(paymentMethods.Items.Any());
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrievePricingForAllMethods() {
            // When: retrieving the ideal method we can include the issuers
            ListResponse<PaymentMethodResponse> paymentMethods = await this._paymentMethodClient.GetAllPaymentMethodListAsync(includePricing: true);

            // Then: We should have prices available
            Assert.IsTrue(paymentMethods.Items.Any(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0)));
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrieveIssuersForAllMethods() {
            // When: retrieving the all mollie payment methods we can include the issuers
            ListResponse<PaymentMethodResponse> paymentMethods = await this._paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true);

            // Then: We should have one or multiple issuers
            Assert.IsTrue(paymentMethods.Items.Any(x => x.Issuers != null));
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrieveIssuersAndPricingInformation() {
            // When: retrieving the all mollie payment methods we can include the issuers
            ListResponse<PaymentMethodResponse> paymentMethods = await this._paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true, includePricing: true);
            
            // Then: We should have one or multiple issuers
            Assert.IsTrue(paymentMethods.Items.Any(x => x.Issuers != null));
            Assert.IsTrue(paymentMethods.Items.Any(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0)));
        }
        
        [RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        [TestCase("JPY", 249)]
        [TestCase("ISK", 50)]
        [TestCase("EUR", 50.25)]
        public async Task GetPaymentMethodListAsync_WithVariousCurrencies_ReturnsAvailablePaymentMethods(string currency, decimal value) {
            // When: Retrieving the payment methods for a currency and amount
            var amount = new Amount(currency, value);
            var paymentMethods = await this._paymentMethodClient.GetPaymentMethodListAsync(amount: amount);
            
            // Then: We should have multiple payment methods
            Assert.IsTrue(paymentMethods.Count > 0);
        }
    }
}