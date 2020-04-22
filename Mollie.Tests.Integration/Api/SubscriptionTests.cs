using System;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Subscription;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class SubscriptionTests : BaseMollieApiTestClass {
        [Test][CustomRetry(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrieveSubscriptionList() {
            // Given
            string customerId = await this.GetFirstCustomerWithValidMandate();

            // When: Retrieve subscription list with default settings
            ListResponse<SubscriptionResponse> response = await this._subscriptionClient.GetSubscriptionListAsync(customerId);

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }

        [Test][CustomRetry(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task ListSubscriptionsNeverReturnsMoreCustomersThenTheNumberOfRequestedSubscriptions() {
            // Given: Number of customers requested is 5
            string customerId = await this.GetFirstCustomerWithValidMandate();
            int numberOfSubscriptions = 5;

            // When: Retrieve 5 subscriptions
            ListResponse<SubscriptionResponse> response = await this._subscriptionClient.GetSubscriptionListAsync(customerId, null, numberOfSubscriptions);

            // Then
            Assert.IsTrue(response.Items.Count <= numberOfSubscriptions);
        }

        [Test][CustomRetry(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreateSubscription() {
            // Given
            string customerId = await this.GetFirstCustomerWithValidMandate();
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest();
            subscriptionRequest.Amount = new Amount(Currency.EUR, "100.00");
            subscriptionRequest.Times = 5;
            subscriptionRequest.Interval = "1 month";
            subscriptionRequest.Description = $"Subscription {Guid.NewGuid()}"; // Subscriptions must have a unique name
            subscriptionRequest.WebhookUrl = "http://www.google.nl";
            subscriptionRequest.StartDate = DateTime.Now.AddDays(1);

            // When
            SubscriptionResponse subscriptionResponse = await this._subscriptionClient.CreateSubscriptionAsync(customerId, subscriptionRequest);

            // Then
            Assert.AreEqual(subscriptionRequest.Amount.Value, subscriptionResponse.Amount.Value);
            Assert.AreEqual(subscriptionRequest.Amount.Currency, subscriptionResponse.Amount.Currency);
            Assert.AreEqual(subscriptionRequest.Times, subscriptionResponse.Times);
            Assert.AreEqual(subscriptionRequest.Interval, subscriptionResponse.Interval);
            Assert.AreEqual(subscriptionRequest.Description, subscriptionResponse.Description);
            Assert.AreEqual(subscriptionRequest.WebhookUrl, subscriptionResponse.WebhookUrl);
            Assert.AreEqual(subscriptionRequest.StartDate.Value.Date, subscriptionResponse.StartDate);
        }

        [Test][CustomRetry(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCancelSubscription() {
            // Given
            string customerId = await this.GetFirstCustomerWithValidMandate();
            ListResponse<SubscriptionResponse> subscriptions = await this._subscriptionClient.GetSubscriptionListAsync(customerId);

            // When
            if (subscriptions.Count > 0) {
                string subscriptionId = subscriptions.Items.First().Id;
                await this._subscriptionClient.CancelSubscriptionAsync(customerId, subscriptionId);
                SubscriptionResponse cancelledSubscription = await this._subscriptionClient.GetSubscriptionAsync(customerId, subscriptionId);

                // Then
                Assert.AreEqual(cancelledSubscription.Status, SubscriptionStatus.Canceled);
            }
            else {
                Assert.Inconclusive("No subscriptions found that could be cancelled");
            }
        }

        [Test][CustomRetry(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanUpdateSubscription() {
            // Given 
            string customerId = await this.GetFirstCustomerWithValidMandate();
            ListResponse<SubscriptionResponse> subscriptions = await this._subscriptionClient.GetSubscriptionListAsync(customerId);

            // When
            if (subscriptions.Count > 0) {
                string subscriptionId = subscriptions.Items.First().Id;
                SubscriptionUpdateRequest request = new SubscriptionUpdateRequest() {
                    Description = $"Updated subscription {Guid.NewGuid()}"
                };
                SubscriptionResponse response = await this._subscriptionClient.UpdateSubscriptionAsync(customerId, subscriptionId, request);

                // Then
                Assert.AreEqual(request.Description, response.Description);
            }
            else {
                Assert.Inconclusive("No subscriptions found that could be cancelled");
            }
        }

        [Test][CustomRetry(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreateSubscriptionWithMetaData() {
            // If: We create a subscription with meta data
            string json = "{\"order_id\":\"4.40\"}";
            string customerId = await this.GetFirstCustomerWithValidMandate();
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest();
            subscriptionRequest.Amount = new Amount(Currency.EUR, "100.00");
            subscriptionRequest.Times = 5;
            subscriptionRequest.Interval = "1 month";
            subscriptionRequest.Description = $"Subscription {Guid.NewGuid()}"; // Subscriptions must have a unique name
            subscriptionRequest.WebhookUrl = "http://www.google.nl";
            subscriptionRequest.StartDate = DateTime.Now.AddDays(1);
            subscriptionRequest.Metadata = json;

            // When We send the subscription request to Mollie
            SubscriptionResponse result = await this._subscriptionClient.CreateSubscriptionAsync(customerId, subscriptionRequest);

            // Then: Make sure we get the same json result as metadata
            Assert.AreEqual(json, result.Metadata);
        }

        public async Task<string> GetFirstCustomerWithValidMandate() {
            ListResponse<CustomerResponse> customers = await this._customerClient.GetCustomerListAsync();
            
            foreach (CustomerResponse customer in customers.Items) {
                ListResponse<MandateResponse> mandates = await this._mandateClient.GetMandateListAsync(customer.Id);
                if (mandates.Items.Any(x => x.Status == MandateStatus.Valid)) {
                    return customer.Id;
                }
            }

            Assert.Inconclusive("No customers with valid mandate found. Unable to test subscription API");
            return null;
        }
    }
}
