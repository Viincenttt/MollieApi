using System;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Subscription;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class SubscriptionTests : BaseMollieApiTestClass {
        [Test]
        public void CanRetrieveMandateList() {
            // Given
            string customerId = this.GetFirstCustomerWithValidMandate();

            // When: Retrieve subscription list with default settings
            ListResponse<SubscriptionResponse> response = this._mollieClient.GetSubscriptionListAsync(customerId).Result;

            // Then
            Assert.IsNotNull(response);
        }

        [Test]
        public void ListSubscriptionsNeverReturnsMoreCustomersThenTheNumberOfRequestedSubscriptions() {
            // Given: Number of customers requested is 5
            string customerId = this.GetFirstCustomerWithValidMandate();
            int numberOfSubscriptions = 5;

            // When: Retrieve 5 subscriptions
            ListResponse<SubscriptionResponse> response = this._mollieClient.GetSubscriptionListAsync(customerId, 0, numberOfSubscriptions).Result;

            // Then
            Assert.IsTrue(response.Data.Count <= numberOfSubscriptions);
        }

        [Test]
        public void CanCreateSubscription() {
            // Given
            string customerId = this.GetFirstCustomerWithValidMandate();
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest();
            subscriptionRequest.Amount = 100;
            subscriptionRequest.Times = 5;
            subscriptionRequest.Interval = "1 month";
            subscriptionRequest.Description = $"Subscription {DateTime.Now}"; // Subscriptions must have a unique name
            subscriptionRequest.WebhookUrl = "http://www.google.nl";

            // When
            SubscriptionResponse subscriptionResponse = this._mollieClient.CreateSubscriptionAsync(customerId, subscriptionRequest).Result;

            // Then
            Assert.AreEqual(subscriptionRequest.Amount, subscriptionResponse.Amount);
            Assert.AreEqual(subscriptionRequest.Times, subscriptionResponse.Times);
            Assert.AreEqual(subscriptionRequest.Interval, subscriptionResponse.Interval);
            Assert.AreEqual(subscriptionRequest.Description, subscriptionResponse.Description);
            Assert.AreEqual(subscriptionRequest.WebhookUrl, subscriptionResponse.Links.WebhookUrl);
        }

        [Test]
        public async Task CanCancelSubscription() {
            // Given
            string customerId = this.GetFirstCustomerWithValidMandate();
            ListResponse<SubscriptionResponse> subscriptions = this._mollieClient.GetSubscriptionListAsync(customerId).Result;

            // When
            if (subscriptions.TotalCount > 0) {
                string subscriptionId = subscriptions.Data.First().Id;
                await this._mollieClient.CancelSubscriptionAsync(customerId, subscriptionId);
                SubscriptionResponse cancelledSubscription = this._mollieClient.GetSubscriptionAsync(customerId, subscriptionId).Result;

                Assert.AreEqual(cancelledSubscription.Status, SubscriptionStatus.Cancelled);
            }
            else {
                Assert.Inconclusive("No subscriptions found that could be cancelled");
            }
        }

        public string GetFirstCustomerWithValidMandate() {
            ListResponse<CustomerResponse> customers = this._mollieClient.GetCustomerListAsync().Result;
            
            foreach (CustomerResponse customer in customers.Data) {
                ListResponse<MandateResponse> mandates = this._mollieClient.GetCustomerMandateListAsync(customer.Id).Result;
                if (mandates.Data.Any(x => x.Status == MandateStatus.Valid)) {
                    return customer.Id;
                }
            }

            Assert.Inconclusive("No customers with valid mandate found. Unable to test subscription API");
            return null;
        }
    }
}
