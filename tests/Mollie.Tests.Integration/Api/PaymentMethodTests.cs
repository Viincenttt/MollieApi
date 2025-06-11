using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class PaymentMethodTests : BaseMollieApiTestClass, IDisposable {
    private readonly IPaymentMethodClient _paymentMethodClient;

    public PaymentMethodTests(IPaymentMethodClient paymentMethodClient) {
        _paymentMethodClient = paymentMethodClient;
    }

    [Fact]
    public async Task CanRetrievePaymentMethodList() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentMethodResponse> response = await _paymentMethodClient.GetPaymentMethodListAsync();

        // Then: Make sure it can be retrieved
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanRetrievePaymentMethodListIncludeWallets() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentMethodResponse> response = await _paymentMethodClient.GetPaymentMethodListAsync(includeWallets: "applepay");

        // Then: Make sure it can be retrieved
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(PaymentMethod.GooglePay)]
    public async Task CanRetrieveSinglePaymentMethod(string method) {
        // When: retrieving a payment method
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(method);

        // Then: Make sure it can be retrieved
        paymentMethod.ShouldNotBeNull();
        paymentMethod.Id.ShouldBe(method);
    }

    [Fact]
    public async Task CanRetrieveKbcIssuers() {
        // When: retrieving the ideal method we can include the issuers
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc, true);

        // Then: We should have one or multiple issuers
        paymentMethod.ShouldNotBeNull();
        paymentMethod.Issuers.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task DoNotRetrieveIssuersWhenIncludeIsFalse() {
        // When: retrieving the ideal method with the include parameter set to false
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc);

        // Then: Issuers should not be included
        paymentMethod.Issuers.ShouldBeNull();
    }

    [Fact]
    public async Task CanRetrieveAllMethods() {
        // When: retrieving the all mollie payment methods
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync();

        // Then: We should have multiple issuers
        paymentMethods.ShouldNotBeNull();
        paymentMethods.Items.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task CanRetrievePricingForAllMethods() {
        // When: retrieving the ideal method we can include the issuers
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync(includePricing: true);

        // Then: We should have prices available
        paymentMethods.Items.All(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0)).ShouldBeTrue();
    }

    [Fact]
    public async Task CanRetrieveIssuersForAllMethods() {
        // When: retrieving the all mollie payment methods we can include the issuers
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true);

        // Then: We should have one or multiple issuers
        paymentMethods.Items.ShouldContain(x => x.Issuers != null);
    }

    [Fact]
    public async Task CanRetrieveIssuersAndPricingInformation() {
        // When: retrieving the all mollie payment methods we can include the issuers
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true, includePricing: true);

        // Then: We should have one or multiple issuers
        paymentMethods.Items.ShouldContain(x => x.Issuers != null);
        paymentMethods.Items.ShouldContain(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0));
    }

    [Theory]
    [InlineData("JPY", 249)]
    [InlineData("EUR", 50.25)]
    public async Task GetPaymentMethodListAsync_WithVariousCurrencies_ReturnsAvailablePaymentMethods(string currency, decimal value) {
        // When: Retrieving the payment methods for a currency and amount
        var amount = new Amount(currency, value);
        var paymentMethods = await _paymentMethodClient.GetPaymentMethodListAsync(amount: amount);

        // Then: We should have multiple payment methods
        paymentMethods.Count.ShouldBeGreaterThan(0);
    }

    public void Dispose()
    {
        _paymentMethodClient?.Dispose();
    }
}
