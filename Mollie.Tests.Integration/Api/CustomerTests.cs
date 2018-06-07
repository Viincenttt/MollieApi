using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
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
        public async Task CanRetrieveRecentPaymentMethods() {
            // If: We create a new customer and create several payment for the customer
            CustomerResponse newCustomer = await this.CreateCustomer("Smit", "johnsmit@mollie.com");
            await this.CreatePayment(newCustomer.Id, PaymentMethod.BankTransfer);
            await this.CreatePayment(newCustomer.Id, PaymentMethod.Ideal);
            await this.CreatePayment(newCustomer.Id, PaymentMethod.CreditCard);

            // When: retrieving the customer again
            CustomerResponse customerResponse = await this._customerClient.GetCustomerAsync(newCustomer.Id);

            // Then recent payment methods should contain the payment method
            Assert.IsNotNull(customerResponse.RecentlyUsedMethods);
            Assert.IsNotNull(customerResponse.RecentlyUsedMethods.FirstOrDefault(x => x == PaymentMethod.BankTransfer));
            Assert.IsNotNull(customerResponse.RecentlyUsedMethods.FirstOrDefault(x => x == PaymentMethod.Ideal));
            Assert.IsNotNull(customerResponse.RecentlyUsedMethods.FirstOrDefault(x => x == PaymentMethod.CreditCard));

        }

        private async Task<PaymentResponse> CreatePayment(string customerId, PaymentMethod method) {
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount("EUR", "100.00"),
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Method = method,
                CustomerId = customerId
            };
            return await this._paymentClient.CreatePaymentAsync(paymentRequest);
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
