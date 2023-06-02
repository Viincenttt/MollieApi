using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api; 

public class CustomerTests : BaseMollieApiTestClass {
    private readonly ICustomerClient _customerClient;
        
    public CustomerTests() {
        _customerClient = new CustomerClient(this.ApiKey);
    }
        
    [DefaultRetryFact]
    public async Task CanRetrieveCustomerList() {
        // When: Retrieve customer list with default settings
        ListResponse<CustomerResponse> response = await this._customerClient.GetCustomerListAsync();

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [DefaultRetryFact]
    public async Task ListCustomersNeverReturnsMoreCustomersThenTheNumberOfRequestedCustomers() {
        // If: Number of customers requested is 5
        int numberOfCustomers = 5;

        // When: Retrieve 5 customers
        ListResponse<CustomerResponse> response = await this._customerClient.GetCustomerListAsync(null, numberOfCustomers);

        // Then
        numberOfCustomers.Should().Be(response.Items.Count);
    }

    [DefaultRetryFact]
    public async Task CanCreateNewCustomer() {
        // If: We create a customer request with only the required parameters
        string name = "Smit";
        string email = "johnsmit@mollie.com";

        // When: We send the customer request to Mollie
        CustomerResponse result = await this.CreateCustomer(name, email);

        // Then: Make sure the requested parameters match the response parameter values
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Email.Should().Be(email);
    }

    [DefaultRetryFact]
    public async Task CanUpdateCustomer() {
        // If: We retrieve the customer list
        ListResponse<CustomerResponse> response = await this._customerClient.GetCustomerListAsync();

        // When: We update one of the customers in the list
        string customerIdToUpdate = response.Items.First().Id;
        string newCustomerName = DateTime.Now.ToShortTimeString();
        CustomerRequest updateParameters = new CustomerRequest() {
            Name = newCustomerName
        };
        CustomerResponse result = await this._customerClient.UpdateCustomerAsync(customerIdToUpdate, updateParameters);

        // Then: Make sure the new name is updated
        result.Should().NotBeNull();
        result.Name.Should().Be(newCustomerName);
    }

    [DefaultRetryFact]
    public async Task CanDeleteCustomer() {
        // If: We retrieve the customer list
        ListResponse<CustomerResponse> response = await this._customerClient.GetCustomerListAsync();

        // When: We delete one of the customers in the list
        string customerIdToDelete = response.Items.First().Id;
        await this._customerClient.DeleteCustomerAsync(customerIdToDelete);

        // Then: Make sure its deleted
        MollieApiException apiException = await Assert.ThrowsAsync<MollieApiException>(() => this._customerClient.GetCustomerAsync(customerIdToDelete));
        apiException.Details.Status.Should().Be((int)HttpStatusCode.Gone);
    }

    [DefaultRetryFact]
    public async Task CanGetCustomerByUrlObject() {
        // If: We create a customer request with only the required parameters
        string name = "Smit";
        string email = "johnsmit@mollie.com";
        CustomerResponse createdCustomer = await this.CreateCustomer(name, email);

        // When: We try to retrieve the customer by Url object
        CustomerResponse retrievedCustomer = await this._customerClient.GetCustomerAsync(createdCustomer.Links.Self);

        // Then: Make sure it's retrieved
        retrievedCustomer.Name.Should().Be(createdCustomer.Name);
        retrievedCustomer.Email.Should().Be(createdCustomer.Email);
    }

    [DefaultRetryFact]
    public async Task CanCreateCustomerWithJsonMetadata() {
        // If: We create a customer request with json metadata
        CustomerRequest customerRequest = new CustomerRequest() {
            Email =  "johnsmit@mollie.com",
            Name = "Smit",
            Metadata =  "{\"order_id\":\"4.40\"}",
            Locale = Locale.nl_NL
        };

        // When: We try to retrieve the customer by Url object
        CustomerResponse retrievedCustomer = await this._customerClient.CreateCustomerAsync(customerRequest);

        // Then: Make sure it's retrieved
        IsJsonResultEqual(customerRequest.Metadata, retrievedCustomer.Metadata).Should().BeTrue();
    }

    [DefaultRetryFact]
    public async Task CanCreateCustomerWithStringMetadata() {
        // If: We create a customer request with string metadata
        CustomerRequest customerRequest = new CustomerRequest() {
            Email = "johnsmit@mollie.com",
            Name = "Smit",
            Metadata = "This is my metadata",
            Locale = Locale.nl_NL
        };

        // When: We try to retrieve the customer by Url object
        CustomerResponse retrievedCustomer = await this._customerClient.CreateCustomerAsync(customerRequest);

        // Then: Make sure it's retrieved
        retrievedCustomer.Metadata.Should().Be(customerRequest.Metadata);
    }

    [DefaultRetryFact]
    public async Task CanCreateNewCustomerPayment() {
        // If: We create a customer request with only the required parameters
        string name = "Smit";
        string email = "johnsmit@mollie.com";
        CustomerResponse customer = await this.CreateCustomer(name, email);
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = this.DefaultRedirectUrl
        };

        // When: We create a payment request for this customer to Mollie
        PaymentResponse paymentResponse = await this._customerClient.CreateCustomerPayment(customer.Id, paymentRequest);

        // Then: Make sure the requested parameters match the response parameter values
        paymentResponse.Should().NotBeNull();
        paymentResponse.CustomerId.Should().Be(customer.Id);
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