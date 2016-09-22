using System;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Payment.Response.Specific;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    using System.Linq;

    using Mollie.Api.Models.Customer;
    using Mollie.Api.Models.Mandate;

    [TestFixture]
    public class PaymentTests : BaseMollieApiTestClass {
        [Test]
        public void CanRetrievePaymentList() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentResponse> response = this._mollieClient.GetPaymentListAsync().Result;

            // Then
            Assert.IsNotNull(response);
        }

        [Test]
        public void ListPaymentsNeverReturnsMorePaymentsThenTheNumberOfRequestedPayments() {
            // If: Number of payments requested is 5
            int numberOfPayments = 5;

            // When: Retrieve 5 payments
            ListResponse<PaymentResponse> response = this._mollieClient.GetPaymentListAsync(0, numberOfPayments).Result;

            // Then
            Assert.IsTrue(response.Data.Count <= numberOfPayments);
        }

        [Test]
        public void CanCreateDefaultPaymentWithOnlyRequiredFields() {
            // If: we create a payment request with only the required parameters
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePaymentAsync(paymentRequest).Result;

            // Then: Make sure we get a valid response
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount, result.Amount);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.Links.RedirectUrl);
        }

        [Test]
        public void CanCreateDefaultPaymentWithAllFields() {
            // If: we create a payment request where all parameters have a value
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Locale = Locale.NL,
                Metadata = @"{""firstName"":""John"",""lastName"":""Doe""}",
                Method = PaymentMethod.BankTransfer,
                WebhookUrl = this.DefaultWebhookUrl
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePaymentAsync(paymentRequest).Result;

            // Then: Make sure all requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount, result.Amount);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.Links.RedirectUrl);
            Assert.AreEqual(paymentRequest.Locale, result.Locale);
            Assert.AreEqual(paymentRequest.Metadata, result.Metadata);
            Assert.AreEqual(paymentRequest.WebhookUrl, result.Links.WebhookUrl);
        }

        [TestCase(typeof(IdealPaymentRequest), PaymentMethod.Ideal, typeof(IdealPaymentResponse))]
        [TestCase(typeof(CreditCardPaymentRequest), PaymentMethod.CreditCard, typeof(CreditCardPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.MisterCash, typeof(MisterCashPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Sofort, typeof(SofortPaymentResponse))]
        [TestCase(typeof(BankTransferPaymentRequest), PaymentMethod.BankTransfer, typeof(BankTransferPaymentResponse))]
        [TestCase(typeof(SepaDirectDebitRequest), PaymentMethod.DirectDebit, typeof(SepaDirectDebitResponse))]
        [TestCase(typeof(PayPalPaymentRequest), PaymentMethod.PayPal, typeof(PayPalPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Bitcoin, typeof(BitcoinPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.PodiumCadeaukaart, typeof(PodiumCadeauKaartPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Belfius, typeof(BelfiusPaymentResponse))]
        [TestCase(typeof(PaymentRequest), null, typeof(PaymentResponse))]
        public void CanCreateSpecificPaymentType(Type paymentType, PaymentMethod? paymentMethod, Type expectedResponseType) {
            // If: we create a specific payment type with some bank transfer specific values
            PaymentRequest paymentRequest = (PaymentRequest) Activator.CreateInstance(paymentType);
            paymentRequest.Amount = 100;
            paymentRequest.Description = "Description";
            paymentRequest.RedirectUrl = this.DefaultRedirectUrl;
            paymentRequest.Method = paymentMethod;

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePaymentAsync(paymentRequest).Result;

            // Then: Make sure all requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponseType, result.GetType());
            Assert.AreEqual(paymentRequest.Amount, result.Amount);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.Links.RedirectUrl);
        }

        [Test]
        public void CanCreatePaymentAndRetrieveIt() {
            // If: we create a new payment request
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Locale = Locale.DE
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = this._mollieClient.CreatePaymentAsync(paymentRequest).Result;
            PaymentResponse result = this._mollieClient.GetPaymentAsync(paymentResponse.Id).Result;

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentResponse.Id, result.Id);
            Assert.AreEqual(paymentResponse.Amount, result.Amount);
            Assert.AreEqual(paymentResponse.Description, result.Description);
            Assert.AreEqual(paymentResponse.Links.RedirectUrl, result.Links.RedirectUrl);
        }

        [Test]
        public void CanCreateRecurringPaymentAndRetrieveIt() {
            // If: we create a new recurring payment
            string customerId = this.GetFirstValidMandate().CustomerId;
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                RecurringType = RecurringType.First,
                CustomerId = customerId
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = this._mollieClient.CreatePaymentAsync(paymentRequest).Result;
            PaymentResponse result = this._mollieClient.GetPaymentAsync(paymentResponse.Id).Result;

            // Then: Make sure the recurringtype parameter is entered
            Assert.AreEqual(RecurringType.First, result.RecurringType);
        }

        [Test]
        public void CanCreatePaymentWithMetaData() {
            // If: We create a payment with meta data
            string json = "{\"order_id\":\"4.40\"}";
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Metadata = json
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePaymentAsync(paymentRequest).Result;

            // Then: Make sure we get the same json result as metadata
            Assert.AreEqual(json, result.Metadata);
        }

        [Test]
        public void CanCreatePaymentWithMandate() {
            // If: We create a payment with a mandate id
            MandateResponse validMandate = this.GetFirstValidMandate();
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                RecurringType = RecurringType.Recurring,
                CustomerId = validMandate.CustomerId,
                MandateId = validMandate.Id
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePaymentAsync(paymentRequest).Result;

            // Then: Make sure we get the mandate id back in the details
            Assert.AreEqual(validMandate.Id, result.MandateId);
        }

        private MandateResponse GetFirstValidMandate() {
            ListResponse<CustomerResponse> customers = this._mollieClient.GetCustomerListAsync().Result;
            if (!customers.Data.Any()) {
                Assert.Inconclusive("No customers found. Unable to test recurring payment tests");
            }

            foreach (CustomerResponse customer in customers.Data) {
                ListResponse<MandateResponse> customerMandates = this._mollieClient.GetMandateListAsync(customer.Id).Result;
                MandateResponse firstValidMandate = customerMandates.Data.FirstOrDefault(x => x.Status == MandateStatus.Valid);
                if (firstValidMandate != null) {
                    return firstValidMandate;
                }
            }

            Assert.Inconclusive("No mandates found. Unable to test recurring payments");
            return null;
        }
    }
}
