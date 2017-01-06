using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class CustomerTests : BaseMollieApiTestClass {
        [Test]
        public void CanRetrieveCustomerList() {
            // When: Retrieve customer list with default settings
            ListResponse<CustomerResponse> response = this._customerClient.GetCustomerListAsync().Result;

            // Then
            Assert.IsNotNull(response);
        }

        [Test]
        public void ListCustomersNeverReturnsMoreCustomersThenTheNumberOfRequestedCustomers() {
            // If: Number of customers requested is 5
            int numberOfCustomers = 5;

            // When: Retrieve 5 customers
            ListResponse<CustomerResponse> response = this._customerClient.GetCustomerListAsync(0, numberOfCustomers).Result;

            // Then
            Assert.IsTrue(response.Data.Count <= numberOfCustomers);
        }

        [Test]
        public void CanCreateNewCustomer() {
            // If: We create a customer request with only the required parameters
            CustomerRequest customerRequest = new CustomerRequest() {
                Email = "test@test.com",
                Name = "Smit",
                Locale = Locale.NL,
                Metadata = "{\"test\":\"test\""
            };

            // When: We send the customer request to Mollie
            CustomerResponse result = this._customerClient.CreateCustomerAsync(customerRequest).Result;

            // then: Make sure the requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(customerRequest.Name, result.Name);
            Assert.AreEqual(customerRequest.Email, result.Email);
            Assert.AreEqual(customerRequest.Metadata, result.Metadata);
        }
    }
}
