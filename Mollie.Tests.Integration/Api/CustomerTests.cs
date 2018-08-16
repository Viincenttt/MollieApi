using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mollie.Api.Client;
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
            Assert.IsTrue(response.Embedded.Items.Count <= numberOfCustomers);
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
            if (response.Embedded.Items.Count == 0) {
                Assert.Inconclusive("No customers found. Unable to test updating customers");
            }

            // When: We update one of the customers in the list
            string customerIdToUpdate = response.Embedded.Items.First().Id;
            string newCustomerName = DateTime.Now.ToShortTimeString();
            CustomerRequest updateParameters = new CustomerRequest() {
                Name = newCustomerName
            };
            CustomerResponse result = await this._customerClient.UpdateCustomerAsync(customerIdToUpdate, updateParameters);

            // Then: Make sure the new name is updated
            Assert.IsNotNull(result);
            Assert.AreEqual(newCustomerName, result.Name);
        }

        [Test]
        public async Task CanDeleteCustomer() {
            // If: We retrieve the customer list
            ListResponse<CustomerListData> response = await this._customerClient.GetCustomerListAsync();
            if (response.Embedded.Items.Count == 0) {
                Assert.Inconclusive("No customers found. Unable to test deleting customers");
            }

            // When: We delete one of the customers in the list
            string customerIdToDelete = response.Embedded.Items.First().Id;
            await this._customerClient.DeleteCustomerAsync(customerIdToDelete);

            // Then: Make sure its deleted
            AggregateException aggregateException = Assert.Throws<AggregateException>(() => this._customerClient.GetCustomerAsync(customerIdToDelete).Wait());
            MollieApiException mollieApiException = aggregateException.InnerExceptions.FirstOrDefault(x => x.GetType() == typeof(MollieApiException)) as MollieApiException;
            Assert.AreEqual((int)HttpStatusCode.Gone, mollieApiException.Details.Status);
        }

        [Test]
        public async Task CanGetCustomerByUrlObject() {
            // If: We create a customer request with only the required parameters
            string name = "Smit";
            string email = "johnsmit@mollie.com";
            CustomerResponse createdCustomer = await this.CreateCustomer(name, email);

            // When: We try to retrieve the customer by Url object
            CustomerResponse retrievedCustomer = await this._customerClient.GetCustomerAsync(createdCustomer.Links.Self);

            // Then: Make sure it's retrieved
            Assert.AreEqual(createdCustomer.Name, retrievedCustomer.Name);
            Assert.AreEqual(createdCustomer.Email, retrievedCustomer.Email);
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
