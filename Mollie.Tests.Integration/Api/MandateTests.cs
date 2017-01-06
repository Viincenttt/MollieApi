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
    }
}
