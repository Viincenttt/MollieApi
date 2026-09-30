using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Mandate.Request.PaymentSpecificParameters;
using Mollie.Api.Models.Mandate.Response.PaymentSpecificParameters;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class MandateTests : BaseMollieApiTestClass, IDisposable {
    private readonly IMandateClient _mandateClient;
    private readonly ICustomerClient _customerClient;

    public MandateTests(IMandateClient mandateClient, ICustomerClient customerClient) {
        _mandateClient = mandateClient;
        _customerClient = customerClient;
    }

    [Fact]
    public async Task CanRetrieveMandateList() {
        // We can only test this if there are customers
        var customerListResult = await _customerClient.GetCustomerListAsync();
        var customers = customerListResult.Data!;

        if (customers.Count > 0) {
            // When: Retrieve mandate list with default settings
            var result = await _mandateClient.GetMandateListAsync(customers.Items.First().Id);
            var response = result.Data!;

            // Then
            customerListResult.Success.ShouldBeTrue();
            result.Success.ShouldBeTrue();
            response.ShouldNotBeNull();
            response.Items.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task ListMandatesNeverReturnsMoreCustomersThenTheNumberOfRequestedMandates() {
        // We can only test this if there are customers
        var customerListResult = await _customerClient.GetCustomerListAsync();
        var customers = customerListResult.Data!;

        if (customers.Count > 0) {
            // If: Number of customers requested is 5
            int numberOfMandates = 5;

            // When: Retrieve 5 mandates
            var result = await _mandateClient.GetMandateListAsync(customers.Items.First().Id, null, numberOfMandates);
            var response = result.Data!;

            // Then
            customerListResult.Success.ShouldBeTrue();
            result.Success.ShouldBeTrue();
            numberOfMandates.ShouldBeGreaterThanOrEqualTo(response.Items.Count);
        }
    }

    [Fact]
    public async Task CanCreateSepaDirectDebitMandate() {
        // We can only test this if there are customers
        var customerListResult = await _customerClient.GetCustomerListAsync();
        var customers = customerListResult.Data!;
        if (customers.Count > 0) {
            // If: We create a new mandate request
            SepaDirectDebitMandateRequest mandateRequest = new () {
                ConsumerAccount = "NL26ABNA0516682814",
                ConsumerName = "John Doe",
                Method = PaymentMethod.DirectDebit
            };

            // When: We send the mandate request
            var result = await _mandateClient.CreateMandateAsync(customers.Items.First().Id, mandateRequest);
            var mandateResponse = result.Data!;

            // Then: Make sure we created a new mandate
            customerListResult.Success.ShouldBeTrue();
            result.Success.ShouldBeTrue();
            mandateResponse.ShouldBeOfType<SepaDirectDebitMandateResponse>();
            var sepaDirectDebitResponse = (SepaDirectDebitMandateResponse)mandateResponse;
            sepaDirectDebitResponse.Details.ConsumerAccount.ShouldBe(mandateRequest.ConsumerAccount);
            sepaDirectDebitResponse.Details.ConsumerName.ShouldBe(mandateRequest.ConsumerName);
        }
    }

    public void Dispose()
    {
        _mandateClient?.Dispose();
        _customerClient?.Dispose();
    }
}
