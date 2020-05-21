using System;
using System.Net.Http;
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
        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrievePaymentList() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentResponse> response = await this._paymentClient.GetPaymentListAsync();

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task ListPaymentsNeverReturnsMorePaymentsThenTheNumberOfRequestedPayments() {
            // When: Number of payments requested is 5
            int numberOfPayments = 5;

            // When: Retrieve 5 payments
            ListResponse<PaymentResponse> response = await this._paymentClient.GetPaymentListAsync(null, numberOfPayments);

            // Then
            Assert.IsTrue(response.Items.Count <= numberOfPayments);
        }

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task WhenRetrievingAListOfPaymentsPaymentSubclassesShouldBeInitialized() {
            // Given: We create a new payment 
            IdealPaymentRequest paymentRequest = new IdealPaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl
            };
            await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // When: We retrieve it in a list
            ListResponse<PaymentResponse> result = await this._paymentClient.GetPaymentListAsync(null, 5);

            // Then: We expect a list with a single ideal payment            
            Assert.IsAssignableFrom<IdealPaymentResponse>(result.Items.First());
        }

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreateDefaultPaymentWithOnlyRequiredFields() {
            // When: we create a payment request with only the required parameters
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
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
        
        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreateDefaultPaymentWithAllFields() {
            // If: we create a payment request where all parameters have a value
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Locale = Locale.nl_NL,
                Metadata = "{\"firstName\":\"John\",\"lastName\":\"Doe\"}",
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
        [TestCase(typeof(PaymentRequest), PaymentMethod.Bancontact, typeof(BancontactPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Sofort, typeof(SofortPaymentResponse))]
        [TestCase(typeof(BankTransferPaymentRequest), PaymentMethod.BankTransfer, typeof(BankTransferPaymentResponse))]
        [TestCase(typeof(PayPalPaymentRequest), PaymentMethod.PayPal, typeof(PayPalPaymentResponse))]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Belfius, typeof(BelfiusPaymentResponse))]
        [TestCase(typeof(KbcPaymentRequest), PaymentMethod.Kbc, typeof(KbcPaymentResponse))]
        [TestCase(typeof(PaymentRequest), null, typeof(PaymentResponse))]
        //[TestCase(typeof(Przelewy24PaymentRequest), PaymentMethod.Przelewy24, typeof(PaymentResponse))] // Payment option is not enabled in website profile
        public async Task CanCreateSpecificPaymentType(Type paymentType, PaymentMethod? paymentMethod, Type expectedResponseType) {
            // If: we create a specific payment type with some bank transfer specific values
            PaymentRequest paymentRequest = (PaymentRequest) Activator.CreateInstance(paymentType);
            paymentRequest.Amount = new Amount(Currency.EUR, "100.00");
            paymentRequest.Description = "Description";
            paymentRequest.RedirectUrl = this.DefaultRedirectUrl;
            paymentRequest.Method = paymentMethod;

            // Set required billing email for Przelewy24
            if (paymentRequest is Przelewy24PaymentRequest request)
            {
                request.BillingEmail = "example@example.com";
            }

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

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentAndRetrieveIt() {
            // If: we create a new payment request
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
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

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreateRecurringPaymentAndRetrieveIt() {
            // If: we create a new recurring payment
            MandateResponse mandate = await this.GetFirstValidMandate();
            CustomerResponse customer = await this._customerClient.GetCustomerAsync(mandate.Links.Customer);
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                SequenceType = SequenceType.First,
                CustomerId = customer.Id
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = await this._paymentClient.CreatePaymentAsync(paymentRequest);
            PaymentResponse result = await this._paymentClient.GetPaymentAsync(paymentResponse.Id);

            // Then: Make sure the recurringtype parameter is entered
            Assert.AreEqual(SequenceType.First, result.SequenceType);
        }

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentWithMetaData() {
            // If: We create a payment with meta data
            string metadata = "this is my metadata";
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Metadata = metadata
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure we get the same json result as metadata
            Assert.AreEqual(metadata, result.Metadata);
        }

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentWithJsonMetaData() {
            // If: We create a payment with meta data
            string json = "{\"order_id\":\"4.40\"}";
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Metadata = json
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure we get the same json result as metadata
            Assert.AreEqual(json, result.Metadata);
        }

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentWithCustomMetaDataClass() {
            // If: We create a payment with meta data
            CustomMetadataClass metadataRequest = new CustomMetadataClass() {
                OrderId = 1,
                Description = "Custom description"
            };

            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
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

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentWithMandate() {
            // If: We create a payment with a mandate id
            MandateResponse validMandate = await this.GetFirstValidMandate();
            CustomerResponse customer = await this._customerClient.GetCustomerAsync(validMandate.Links.Customer);
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                SequenceType = SequenceType.Recurring,
                CustomerId = customer.Id,
                MandateId = validMandate.Id
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure we get the mandate id back in the details
            Assert.AreEqual(validMandate.Id, result.MandateId);
        }

        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task PaymentWithDifferentHttpInstance() {
            // If: We create a PaymentClient with our own HttpClient instance
            HttpClient myHttpClientInstance = new HttpClient();
            PaymentClient paymentClient = new PaymentClient(this.GetApiKeyFromConfiguration(), myHttpClientInstance);
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl
            };

            // When: I create a new payment
            PaymentResponse result = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: It should still work... lol
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(paymentRequest.Amount.Value, result.Amount.Value);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.RedirectUrl);
        }

        [Test]
        [RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentWithDecimalAmountAndRetrieveIt()
        {
            // If: we create a new payment request
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, 100.1235m),
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
        [RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentWithImplicitAmountCastAndRetrieveIt()
        {
            var initialAmount = 100.75m;

            // If: we create a new payment request
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, initialAmount),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Locale = Locale.de_DE
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = await this._paymentClient.CreatePaymentAsync(paymentRequest);
            PaymentResponse result = await this._paymentClient.GetPaymentAsync(paymentResponse.Id);

            decimal responseAmount = paymentResponse.Amount; // Implicit cast
            decimal resultAmount = result.Amount; // Implicit cast

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentResponse.Id, result.Id);
            Assert.AreEqual(paymentResponse.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(paymentResponse.Amount.Value, result.Amount.Value);
            Assert.AreEqual(responseAmount, resultAmount);
            Assert.AreEqual(initialAmount, resultAmount);
            Assert.AreEqual(paymentResponse.Description, result.Description);
            Assert.AreEqual(paymentResponse.RedirectUrl, result.RedirectUrl);
        }

        private async Task<MandateResponse> GetFirstValidMandate() {
            ListResponse<CustomerResponse> customers = await this._customerClient.GetCustomerListAsync();
            if (!customers.Items.Any()) {
                Assert.Inconclusive("No customers found. Unable to test recurring payment tests");
            }

            foreach (CustomerResponse customer in customers.Items) {
                ListResponse<MandateResponse> customerMandates = await this._mandateClient.GetMandateListAsync(customer.Id);
                MandateResponse firstValidMandate = customerMandates.Items.FirstOrDefault(x => x.Status == MandateStatus.Valid);
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
