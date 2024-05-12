﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Mandate.Request;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;

namespace Mollie.Tests.Integration.Api;

public class MandateTests : BaseMollieApiTestClass, IDisposable {
    private readonly IMandateClient _mandateClient;
    private readonly ICustomerClient _customerClient;

    public MandateTests() {
        _mandateClient = new MandateClient(ApiKey);
        _customerClient = new CustomerClient(ApiKey);
    }

    [DefaultRetryFact]
    public async Task CanRetrieveMandateList() {
        // We can only test this if there are customers
        ListResponse<CustomerResponse> customers = await _customerClient.GetCustomerListAsync();

        if (customers.Count > 0) {
            // When: Retrieve mandate list with default settings
            ListResponse<MandateResponse> response = await _mandateClient.GetMandateListAsync(customers.Items.First().Id);

            // Then
            response.Should().NotBeNull();
            response.Items.Should().NotBeNull();
        }
    }

    [DefaultRetryFact]
    public async Task ListMandatesNeverReturnsMoreCustomersThenTheNumberOfRequestedMandates() {
        // We can only test this if there are customers
        ListResponse<CustomerResponse> customers = await _customerClient.GetCustomerListAsync();

        if (customers.Count > 0) {
            // If: Number of customers requested is 5
            int numberOfMandates = 5;

            // When: Retrieve 5 mandates
            ListResponse<MandateResponse> response = await _mandateClient.GetMandateListAsync(customers.Items.First().Id, null, numberOfMandates);

            // Then
            numberOfMandates.Should().BeGreaterOrEqualTo(response.Items.Count);
        }
    }

    [DefaultRetryFact]
    public async Task CanCreateMandate() {
        // We can only test this if there are customers
        ListResponse<CustomerResponse> customers = await _customerClient.GetCustomerListAsync();
        if (customers.Count > 0) {
            // If: We create a new mandate request
            SepaDirectDebitMandateRequest mandateRequest = new SepaDirectDebitMandateRequest {
                ConsumerAccount = "NL26ABNA0516682814",
                ConsumerName = "John Doe",
                Method = PaymentMethod.DirectDebit
            };

            // When: We send the mandate request
            MandateResponse mandateResponse = await _mandateClient.CreateMandateAsync(customers.Items.First().Id, mandateRequest);

            // Then: Make sure we created a new mandate
            mandateResponse.Details.ConsumerAccount.Should().Be(mandateRequest.ConsumerAccount);
            mandateResponse.Details.ConsumerName.Should().Be(mandateRequest.ConsumerName);
        }
    }

    public void Dispose()
    {
        _mandateClient?.Dispose();
        _customerClient?.Dispose();
    }
}
