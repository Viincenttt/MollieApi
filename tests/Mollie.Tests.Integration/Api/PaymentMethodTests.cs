using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
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
        var result = await _paymentMethodClient.GetPaymentMethodListAsync();
        var response = result.Data!;

        // Then: Make sure it can be retrieved
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanRetrievePaymentMethodListIncludeWallets() {
        // When: Retrieve payment list with default settings
        var result = await _paymentMethodClient.GetPaymentMethodListAsync(includeWallets: "applepay");
        var response = result.Data!;

        // Then: Make sure it can be retrieved
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(PaymentMethod.GooglePay)]
    public async Task CanRetrieveSinglePaymentMethod(string method) {
        // When: retrieving a payment method
        var result = await _paymentMethodClient.GetPaymentMethodAsync(method);
        var paymentMethod = result.Data!;

        // Then: Make sure it can be retrieved
        result.Success.ShouldBeTrue();
        paymentMethod.ShouldNotBeNull();
        paymentMethod.Id.ShouldBe(method);
    }

    [Fact]
    public async Task CanRetrieveKbcIssuers() {
        // When: retrieving the ideal method we can include the issuers
        var result = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc, true);
        var paymentMethod = result.Data!;

        // Then: We should have one or multiple issuers
        result.Success.ShouldBeTrue();
        paymentMethod.ShouldNotBeNull();
        paymentMethod.Issuers.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task DoNotRetrieveIssuersWhenIncludeIsFalse() {
        // When: retrieving the ideal method with the include parameter set to false
        var result = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc);
        var paymentMethod = result.Data!;

        // Then: Issuers should not be included
        result.Success.ShouldBeTrue();
        paymentMethod.Issuers.ShouldBeNull();
    }

    [Fact]
    public async Task CanRetrieveAllMethods() {
        // When: retrieving the all mollie payment methods
        var result = await _paymentMethodClient.GetAllPaymentMethodListAsync();
        var paymentMethods = result.Data!;

        // Then: We should have multiple issuers
        result.Success.ShouldBeTrue();
        paymentMethods.ShouldNotBeNull();
        paymentMethods.Items.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task CanRetrievePricingForAllMethods() {
        // When: retrieving the ideal method we can include the issuers
        var result = await _paymentMethodClient.GetAllPaymentMethodListAsync(includePricing: true);
        var paymentMethods = result.Data!;

        // Then: We should have prices available
        result.Success.ShouldBeTrue();
        paymentMethods.Items.All(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0)).ShouldBeTrue();
    }

    [Fact]
    public async Task CanRetrieveIssuersForAllMethods() {
        // When: retrieving the all mollie payment methods we can include the issuers
        var result = await _paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true);
        var paymentMethods = result.Data!;

        // Then: We should have one or multiple issuers
        result.Success.ShouldBeTrue();
        paymentMethods.Items.ShouldContain(x => x.Issuers != null);
    }

    [Fact]
    public async Task CanRetrieveIssuersAndPricingInformation() {
        // When: retrieving the all mollie payment methods we can include the issuers
        var result = await _paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true, includePricing: true);
        var paymentMethods = result.Data!;

        // Then: We should have one or multiple issuers
        result.Success.ShouldBeTrue();
        paymentMethods.Items.ShouldContain(x => x.Issuers != null);
        paymentMethods.Items.ShouldContain(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0));
    }

    [Theory]
    [InlineData("JPY", 249)]
    [InlineData("EUR", 50.25)]
    public async Task GetPaymentMethodListAsync_WithVariousCurrencies_ReturnsAvailablePaymentMethods(string currency, decimal value) {
        // When: Retrieving the payment methods for a currency and amount
        var amount = new Amount(currency, value);
        var result = await _paymentMethodClient.GetPaymentMethodListAsync(amount: amount);
        var paymentMethods = result.Data!;

        // Then: We should have multiple payment methods
        result.Success.ShouldBeTrue();
        paymentMethods.Count.ShouldBeGreaterThan(0);
    }

    public void Dispose()
    {
        _paymentMethodClient?.Dispose();
    }
}
