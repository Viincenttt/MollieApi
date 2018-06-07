using System;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Subscription;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class SubscriptionTests : BaseMollieApiTestClass {
        [Test]
        public async Task CanRetrieveMandateList() {
            // Given
            string customerId = await this.GetFirstCustomerWithValidMandate();

            // When: Retrieve subscription list with default settings
            ListResponse<SubscriptionResponse> response = await this._subscriptionClient.GetSubscriptionListAsync(customerId);

            // Then
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task ListSubscriptionsNeverReturnsMoreCustomersThenTheNumberOfRequestedSubscriptions() {
            // Given: Number of customers requested is 5
            string customerId = await this.GetFirstCustomerWithValidMandate();
            int numberOfSubscriptions = 5;

            // When: Retrieve 5 subscriptions
            ListResponse<SubscriptionResponse> response = await this._subscriptionClient.GetSubscriptionListAsync(customerId, null, numberOfSubscriptions);

            // Then
            Assert.IsTrue(response.Data.Count <= numberOfSubscriptions);
        }

        [Test]
        public async Task CanCreateSubscription() {
            // Given
            string customerId = await this.GetFirstCustomerWithValidMandate();
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest();
            subscriptionRequest.Amount = 100;
            subscriptionRequest.Times = 5;
            subscriptionRequest.Interval = "1 month";
            subscriptionRequest.Description = $"Subscription {DateTime.Now}"; // Subscriptions must have a unique name
            subscriptionRequest.WebhookUrl = "http://www.google.nl";
            subscriptionRequest.StartDate = DateTime.Now.AddDays(1);

            // When
            SubscriptionResponse subscriptionResponse = await this._subscriptionClient.CreateSubscriptionAsync(customerId, subscriptionRequest);

            // Then
            Assert.AreEqual(subscriptionRequest.Amount, subscriptionResponse.Amount);
            Assert.AreEqual(subscriptionRequest.Times, subscriptionResponse.Times);
            Assert.AreEqual(subscriptionRequest.Interval, subscriptionResponse.Interval);
            Assert.AreEqual(subscriptionRequest.Description, subscriptionResponse.Description);
            Assert.AreEqual(subscriptionRequest.WebhookUrl, subscriptionResponse.Links.WebhookUrl);
            Assert.AreEqual(subscriptionRequest.StartDate.Value.Date, subscriptionResponse.StartDate);
        }

        [Test]
        public async Task CanCancelSubscription() {
            // Given
            string customerId = await this.GetFirstCustomerWithValidMandate();
            ListResponse<SubscriptionResponse> subscriptions = await this._subscriptionClient.GetSubscriptionListAsync(customerId);

            // When
            if (subscriptions.Count > 0) {
                string subscriptionId = subscriptions.Data.First().Id;
                await this._subscriptionClient.CancelSubscriptionAsync(customerId, subscriptionId);
                SubscriptionResponse cancelledSubscription = await this._subscriptionClient.GetSubscriptionAsync(customerId, subscriptionId);

                Assert.AreEqual(cancelledSubscription.Status, SubscriptionStatus.Cancelled);
            }
            else {
                Assert.Inconclusive("No subscriptions found that could be cancelled");
            }
        }

        public async Task<string> GetFirstCustomerWithValidMandate() {
            ListResponse<CustomerListData> customers = await this._customerClient.GetCustomerListAsync();
            
            foreach (CustomerResponse customer in customers.Embedded.Customers) {
                ListResponse<MandateListData> mandates = await this._mandateClient.GetMandateListAsync(customer.Id);
                if (mandates.Embedded.Mandates.Any(x => x.Status == MandateStatus.Valid)) {
                    return customer.Id;
                }
            }

            Assert.Inconclusive("No customers with valid mandate found. Unable to test subscription API");
            return null;
        }
    }
}
