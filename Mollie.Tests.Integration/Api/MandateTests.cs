using System.Linq;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class MandateTests : BaseMollieApiTestClass {
        [Test]
        public void CanRetrieveMandateList() {
            // We can only test this if there are customers
            ListResponse<CustomerResponse> customers = this._customerClient.GetCustomerListAsync().Result;

            if (customers.TotalCount > 0) {
                // When: Retrieve mandate list with default settings
                ListResponse<MandateResponse> response = this._mandateClient.GetMandateListAsync(customers.Data.First().Id).Result;

                // Then
                Assert.IsNotNull(response);
            }
        }

        [Test]
        public void ListMandatesNeverReturnsMoreCustomersThenTheNumberOfRequestedMandates() {
            // We can only test this if there are customers
            ListResponse<CustomerResponse> customers = this._customerClient.GetCustomerListAsync().Result;

            if (customers.TotalCount > 0) {
                // If: Number of customers requested is 5
                int numberOfMandates = 5;

                // When: Retrieve 5 mandates
                ListResponse<MandateResponse> response = this._mandateClient.GetMandateListAsync(customers.Data.First().Id, 0, numberOfMandates).Result;

                // Then
                Assert.IsTrue(response.Data.Count <= numberOfMandates);
            }
        }

        [Test]
        public void CanCreateMandate() {
            // We can only test this if there are customers
            ListResponse<CustomerResponse> customers = this._customerClient.GetCustomerListAsync().Result;
            if (customers.TotalCount > 0) {
                // If: We create a new mandate request
                MandateRequest mandateRequest = new MandateRequest() {
                    ConsumerAccount = "NL26ABNA0516682814",
                    ConsumerName = "John Doe"
                };

                // When: We send the mandate request
                MandateResponse mandateResponse = this._mandateClient.CreateMandateAsync(customers.Data.First().Id, mandateRequest).Result;

                // Then: Make sure we created a new mandate
                Assert.AreEqual(mandateRequest.ConsumerAccount, mandateResponse.Details.ConsumerAccount);
                Assert.AreEqual(mandateRequest.ConsumerName, mandateResponse.Details.ConsumerName);
            }
        }
    }
}
