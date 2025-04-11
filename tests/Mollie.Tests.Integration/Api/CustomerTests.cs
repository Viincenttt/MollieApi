using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer.Request;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class CustomerTests : BaseMollieApiTestClass, IDisposable {
    private readonly ICustomerClient _customerClient;

    public CustomerTests(ICustomerClient customerClient) {
        _customerClient = customerClient;
    }

    [Fact]
    public async Task CanRetrieveCustomerList() {
        // When: Retrieve customer list with default settings
        ListResponse<CustomerResponse> response = await _customerClient.GetCustomerListAsync();

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task ListCustomersNeverReturnsMoreCustomersThenTheNumberOfRequestedCustomers() {
        // If: Number of customers requested is 5
        int numberOfCustomers = 5;

        // When: Retrieve 5 customers
        ListResponse<CustomerResponse> response = await _customerClient.GetCustomerListAsync(null, numberOfCustomers);

        // Then
        numberOfCustomers.ShouldBe(response.Items.Count);
    }

    [Fact]
    public async Task CanCreateNewCustomer() {
        // If: We create a customer request with only the required parameters
        string name = "Smit";
        string email = "johnsmit@mollie.com";

        // When: We send the customer request to Mollie
        CustomerResponse result = await CreateCustomer(name, email);

        // Then: Make sure the requested parameters match the response parameter values
        result.ShouldNotBeNull();
        result.Name.ShouldBe(name);
        result.Email.ShouldBe(email);
    }

    [Fact]
    public async Task CanUpdateCustomer() {
        // If: We retrieve the customer list
        ListResponse<CustomerResponse> response = await _customerClient.GetCustomerListAsync();

        // When: We update one of the customers in the list
        string customerIdToUpdate = response.Items.First().Id;
        string newCustomerName = DateTime.Now.ToShortTimeString();
        CustomerRequest updateParameters = new CustomerRequest() {
            Name = newCustomerName
        };
        CustomerResponse result = await _customerClient.UpdateCustomerAsync(customerIdToUpdate, updateParameters);

        // Then: Make sure the new name is updated
        result.ShouldNotBeNull();
        result.Name.ShouldBe(newCustomerName);
    }

    [Fact]
    public async Task CanDeleteCustomer() {
        // If: We retrieve the customer list
        ListResponse<CustomerResponse> response = await _customerClient.GetCustomerListAsync();

        // When: We delete one of the customers in the list
        string customerIdToDelete = response.Items.First().Id;
        await _customerClient.DeleteCustomerAsync(customerIdToDelete);

        // Then: Make sure its deleted after one second
        await Task.Delay(TimeSpan.FromSeconds(1));
        MollieApiException apiException = await Assert.ThrowsAsync<MollieApiException>(() => _customerClient.GetCustomerAsync(customerIdToDelete));
        apiException.Details.Status.ShouldBe((int)HttpStatusCode.Gone);
    }

    [Fact]
    public async Task CanGetCustomerByUrlObject() {
        // If: We create a customer request with only the required parameters
        string name = "Smit";
        string email = "johnsmit@mollie.com";
        CustomerResponse createdCustomer = await CreateCustomer(name, email);

        // When: We try to retrieve the customer by Url object
        CustomerResponse retrievedCustomer = await ExecuteWithRetry(() => _customerClient.GetCustomerAsync(createdCustomer.Links.Self));

        // Then: Make sure it's retrieved
        retrievedCustomer.Name.ShouldBe(createdCustomer.Name);
        retrievedCustomer.Email.ShouldBe(createdCustomer.Email);
    }

    [Fact]
    public async Task CanCreateCustomerWithJsonMetadata() {
        // If: We create a customer request with json metadata
        var customerRequest = new CustomerRequest() {
            Email =  "johnsmit@mollie.com",
            Name = "Smit",
            Metadata =  "{\"order_id\":\"4.40\"}",
            Locale = Locale.nl_NL
        };

        // When: We try to retrieve the customer by Url object
        CustomerResponse retrievedCustomer = await _customerClient.CreateCustomerAsync(customerRequest);

        // Then: Make sure it's retrieved
        IsJsonResultEqual(customerRequest.Metadata, retrievedCustomer.Metadata).ShouldBeTrue();
    }

    [Fact]
    public async Task CanCreateCustomerWithStringMetadata() {
        // If: We create a customer request with string metadata
        CustomerRequest customerRequest = new CustomerRequest() {
            Email = "johnsmit@mollie.com",
            Name = "Smit",
            Metadata = "This is my metadata",
            Locale = Locale.nl_NL
        };

        // When: We try to retrieve the customer by Url object
        CustomerResponse retrievedCustomer = await _customerClient.CreateCustomerAsync(customerRequest);

        // Then: Make sure it's retrieved
        retrievedCustomer.Metadata.ShouldBe(customerRequest.Metadata);
    }

    [Fact]
    public async Task CanCreateNewCustomerPayment() {
        // If: We create a customer request with only the required parameters
        string name = "Smit";
        string email = "johnsmit@mollie.com";
        CustomerResponse customer = await CreateCustomer(name, email);
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We create a payment request for this customer to Mollie
        PaymentResponse paymentResponse = await _customerClient.CreateCustomerPayment(customer.Id, paymentRequest);

        // Then: Make sure the requested parameters match the response parameter values
        paymentResponse.ShouldNotBeNull();
        paymentResponse.CustomerId.ShouldBe(customer.Id);
    }

    private async Task<CustomerResponse> CreateCustomer(string name, string email) {
        CustomerRequest customerRequest = new CustomerRequest() {
            Email = email,
            Name = name,
            Locale = Locale.nl_NL
        };

        return await _customerClient.CreateCustomerAsync(customerRequest);
    }

    public void Dispose()
    {
        _customerClient?.Dispose();
    }
}
