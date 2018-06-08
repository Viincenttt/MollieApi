using System;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class CustomerTests : BaseMollieApiTestClass {
        [Test]
        public async Task CanRetrieveCustomerList() {
            // When: Retrieve customer list with default settings
            ListResponse<CustomerListData> response = await this._customerClient.GetCustomerListAsync();

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Embedded);
        }

        [Test]
        public async Task ListCustomersNeverReturnsMoreCustomersThenTheNumberOfRequestedCustomers() {
            // If: Number of customers requested is 5
            int numberOfCustomers = 5;

            // When: Retrieve 5 customers
            ListResponse<CustomerListData> response = await this._customerClient.GetCustomerListAsync(null, numberOfCustomers);

            // Then
            Assert.IsTrue(response.Embedded.Customers.Count <= numberOfCustomers);
        }

        [Test]
        public async Task CanCreateNewCustomer() {
            // If: We create a customer request with only the required parameters
            string name = "Smit";
            string email = "johnsmit@mollie.com";

            // When: We send the customer request to Mollie
            CustomerResponse result = await this.CreateCustomer(name, email);

            // Then: Make sure the requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(email, result.Email);
        }

        [Test]
        public async Task CanUpdateCustomer() {
            // If: We retrieve the customer list
            ListResponse<CustomerListData> response = await this._customerClient.GetCustomerListAsync();
            if (response.Embedded.Customers.Count == 0) {
                Assert.Inconclusive("No customers found. Unable to test updating customers");
            }

            // When: We update one of the customers in the list
            string customerIdToUpdate = response.Embedded.Customers.First().Id;
            string newCustomerName = DateTime.Now.ToShortDateString();
            CustomerRequest updateParameters = new CustomerRequest() {
                Name = newCustomerName
            };
            CustomerResponse result = await this._customerClient.UpdateCustomerAsync(customerIdToUpdate, updateParameters);

            // Then: Make sure the new name is updated
            Assert.IsNotNull(result);
            Assert.AreEqual(newCustomerName, result.Name);
        }

        private async Task<CustomerResponse> CreateCustomer(string name, string email) {
            CustomerRequest customerRequest = new CustomerRequest() {
                Email = email,
                Name = name,
                Locale = Locale.nl_NL
            };

            return await this._customerClient.CreateCustomerAsync(customerRequest);
        }
    }
}
