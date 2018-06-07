using System;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
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
        public async Task CanRetrievePaymentList() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentListData> response = await this._paymentClient.GetPaymentListAsync();

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Embedded);
        }

        [Test]
        public async Task ListPaymentsNeverReturnsMorePaymentsThenTheNumberOfRequestedPayments() {
            // If: Number of payments requested is 5
            int numberOfPayments = 5;

            // When: Retrieve 5 payments
            ListResponse<PaymentListData> response = await this._paymentClient.GetPaymentListAsync(null, numberOfPayments);

            // Then
            Assert.IsTrue(response.Data.Count <= numberOfPayments);
        }

        [Test]
        public async Task CanCreateDefaultPaymentWithOnlyRequiredFields() {
            // If: we create a payment request with only the required parameters
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure we get a valid response
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(paymentRequest.Amount.Value, result.Amount.Value);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.RedirectUrl);
        }
        
        [Test]
        public async Task CanCreateDefaultPaymentWithAllFields() {
            // If: we create a payment request where all parameters have a value
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Locale = Locale.nl_NL,
                Metadata = @"{""firstName"":""John"",""lastName"":""Doe""}",
                Method = PaymentMethod.BankTransfer,
                WebhookUrl = this.DefaultWebhookUrl
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure all requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(paymentRequest.Amount.Value, result.Amount.Value);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.RedirectUrl);
            Assert.AreEqual(paymentRequest.Locale, result.Locale);
            Assert.AreEqual(paymentRequest.Metadata, result.Metadata);
            Assert.AreEqual(paymentRequest.WebhookUrl, result.WebhookUrl);
        }

        [TestCase(typeof(IdealPaymentRequest), PaymentMethod.Ideal, typeof(IdealPaymentResponse))]
        [TestCase(typeof(CreditCardPaymentRequest), PaymentMethod.CreditCard, typeof(CreditCardPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Bancontact, typeof(BancontactPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Sofort, typeof(SofortPaymentResponse))]
        [TestCase(typeof(BankTransferPaymentRequest), PaymentMethod.BankTransfer, typeof(BankTransferPaymentResponse))]
        [TestCase(typeof(PayPalPaymentRequest), PaymentMethod.PayPal, typeof(PayPalPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Bitcoin, typeof(BitcoinPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Belfius, typeof(BelfiusPaymentResponse))]
        [TestCase(typeof(KbcPaymentRequest), PaymentMethod.Kbc, typeof(KbcPaymentResponse))]
        [TestCase(typeof(PaymentRequest), null, typeof(PaymentResponse))]
        public async Task CanCreateSpecificPaymentType(Type paymentType, PaymentMethod? paymentMethod, Type expectedResponseType) {
            // If: we create a specific payment type with some bank transfer specific values
            PaymentRequest paymentRequest = (PaymentRequest) Activator.CreateInstance(paymentType);
            paymentRequest.Amount = new Amount("EUR", "100.00");
            paymentRequest.Description = "Description";
            paymentRequest.RedirectUrl = this.DefaultRedirectUrl;
            paymentRequest.Method = paymentMethod;

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure all requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponseType, result.GetType());
            Assert.AreEqual(paymentRequest.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(paymentRequest.Amount.Value, result.Amount.Value);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.RedirectUrl);
        }

        [Test]
        public async Task CanCreatePaymentAndRetrieveIt() {
            // If: we create a new payment request
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Locale = Locale.de_DE
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = await this._paymentClient.CreatePaymentAsync(paymentRequest);
            PaymentResponse result = await this._paymentClient.GetPaymentAsync(paymentResponse.Id);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentResponse.Id, result.Id);
            Assert.AreEqual(paymentResponse.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(paymentResponse.Amount.Value, result.Amount.Value);
            Assert.AreEqual(paymentResponse.Description, result.Description);
            Assert.AreEqual(paymentResponse.RedirectUrl, result.RedirectUrl);
        }

        [Test]
        public async Task CanCreateRecurringPaymentAndRetrieveIt() {
            // If: we create a new recurring payment
            MandateResponse mandate = await this.GetFirstValidMandate();
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                RecurringType = RecurringType.First,
                CustomerId = mandate.CustomerId
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = await this._paymentClient.CreatePaymentAsync(paymentRequest);
            PaymentResponse result = await this._paymentClient.GetPaymentAsync(paymentResponse.Id);

            // Then: Make sure the recurringtype parameter is entered
            Assert.AreEqual(RecurringType.First, result.RecurringType);
        }

        [Test]
        public async Task CanCreatePaymentWithMetaData() {
            // If: We create a payment with meta data
            string json = "{\"order_id\":\"4.40\"}";
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Metadata = json
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure we get the same json result as metadata
            Assert.AreEqual(json, result.Metadata);
        }

        [Test]
        public async Task CanCreatePaymentWithCustomMetaDataClass() {
            // If: We create a payment with meta data
            CustomMetadataClass metadataRequest = new CustomMetadataClass() {
                OrderId = 1,
                Description = "Custom description"
            };

            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
            };
            paymentRequest.SetMetadata(metadataRequest);

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);
            CustomMetadataClass metadataResponse = result.GetMetadata<CustomMetadataClass>();

            // Then: Make sure we get the same json result as metadata
            Assert.IsNotNull(metadataResponse);
            Assert.AreEqual(metadataRequest.OrderId, metadataResponse.OrderId);
            Assert.AreEqual(metadataRequest.Description, metadataResponse.Description);
        }

        [Test]
        public async Task CanCreatePaymentWithMandate() {
            // If: We create a payment with a mandate id
            MandateResponse validMandate = await this.GetFirstValidMandate();
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                RecurringType = RecurringType.Recurring,
                CustomerId = validMandate.CustomerId,
                MandateId = validMandate.Id
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure we get the mandate id back in the details
            Assert.AreEqual(validMandate.Id, result.MandateId);
        }

        [Test]
        public async Task PaymentWithInvalidJsonThrowsException() {
            // If: We create a payment with invalid json
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Metadata = "IAmNotAValidJsonString"
            };

            // When + Then: We send the payment request to Mollie, we expect the exception
            Assert.ThrowsAsync<MollieApiException>(() => this._paymentClient.CreatePaymentAsync(paymentRequest));
        }

        private async Task<MandateResponse> GetFirstValidMandate() {
            ListResponse<CustomerResponse> customers = await this._customerClient.GetCustomerListAsync();
            if (!customers.Data.Any()) {
                Assert.Inconclusive("No customers found. Unable to test recurring payment tests");
            }

            foreach (CustomerResponse customer in customers.Data) {
                ListResponse<MandateResponse> customerMandates = await this._mandateClient.GetMandateListAsync(customer.Id);
                MandateResponse firstValidMandate = customerMandates.Data.FirstOrDefault(x => x.Status == MandateStatus.Valid);
                if (firstValidMandate != null) {
                    return firstValidMandate;
                }
            }

            Assert.Inconclusive("No mandates found. Unable to test recurring payments");
            return null;
        }
    }

    public class CustomMetadataClass {
        public int OrderId { get; set; }
        public string Description { get; set; }
    }
}
