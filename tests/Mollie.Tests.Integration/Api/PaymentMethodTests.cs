using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class PaymentMethodTests : BaseMollieApiTestClass, IDisposable {
    private readonly IPaymentMethodClient _paymentMethodClient;

    public PaymentMethodTests() {
        _paymentMethodClient = new PaymentMethodClient(ApiKey);
    }

    [DefaultRetryFact]
    public async Task CanRetrievePaymentMethodList() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentMethodResponse> response = await _paymentMethodClient.GetPaymentMethodListAsync();

        // Then: Make sure it can be retrieved
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [DefaultRetryFact]
    public async Task CanRetrievePaymentMethodListIncludeWallets() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentMethodResponse> response = await _paymentMethodClient.GetPaymentMethodListAsync(includeWallets: "applepay");

        // Then: Make sure it can be retrieved
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [DefaultRetryTheory]
    [InlineData(PaymentMethod.Bancontact)]
    [InlineData(PaymentMethod.BankTransfer)]
    [InlineData(PaymentMethod.Belfius)]
    [InlineData(PaymentMethod.PayPal)]
    [InlineData(PaymentMethod.Kbc)]
    public async Task CanRetrieveSinglePaymentMethod(string method) {
        // When: retrieving a payment method
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(method);

        // Then: Make sure it can be retrieved
        paymentMethod.Should().NotBeNull();
        paymentMethod.Id.Should().Be(method);
    }

    [DefaultRetryFact]
    public async Task CanRetrieveKbcIssuers() {
        // When: retrieving the ideal method we can include the issuers
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc, true);

        // Then: We should have one or multiple issuers
        paymentMethod.Should().NotBeNull();
        paymentMethod.Issuers.Should().NotBeEmpty();
    }

    [DefaultRetryFact]
    public async Task DoNotRetrieveIssuersWhenIncludeIsFalse() {
        // When: retrieving the ideal method with the include parameter set to false
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Kbc);

        // Then: Issuers should not be included
        paymentMethod.Issuers.Should().BeNull();
    }

    [DefaultRetryFact]
    public async Task CanRetrievePricing() {
        // When: retrieving the ideal method we can include the issuers
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.CreditCard, includePricing: true);

        // Then: We should have one or multiple issuers
        paymentMethod.Should().NotBeNull();
        paymentMethod.Pricing.Should().NotBeEmpty();
    }

    [DefaultRetryFact]
    public async Task DoNotRetrievePricingWhenIncludeIsFalse() {
        // When: retrieving the ideal method with the include parameter set to false
        PaymentMethodResponse paymentMethod = await _paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.CreditCard, includePricing: false);

        // Then: Issuers should not be included
        paymentMethod.Pricing.Should().BeNull();
    }

    [DefaultRetryFact]
    public async Task CanRetrieveAllMethods() {
        // When: retrieving the all mollie payment methods
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync();

        // Then: We should have multiple issuers
        paymentMethods.Should().NotBeNull();
        paymentMethods.Items.Should().NotBeEmpty();
    }

    [DefaultRetryFact]
    public async Task CanRetrievePricingForAllMethods() {
        // When: retrieving the ideal method we can include the issuers
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync(includePricing: true);

        // Then: We should have prices available
        paymentMethods.Items.All(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0)).Should().BeTrue();
    }

    [DefaultRetryFact]
    public async Task CanRetrieveIssuersForAllMethods() {
        // When: retrieving the all mollie payment methods we can include the issuers
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true);

        // Then: We should have one or multiple issuers
        paymentMethods.Items.Should().Contain(x => x.Issuers != null);
    }

    [DefaultRetryFact]
    public async Task CanRetrieveIssuersAndPricingInformation() {
        // When: retrieving the all mollie payment methods we can include the issuers
        ListResponse<PaymentMethodResponse> paymentMethods = await _paymentMethodClient.GetAllPaymentMethodListAsync(includeIssuers: true, includePricing: true);

        // Then: We should have one or multiple issuers
        paymentMethods.Items.Should().Contain(x => x.Issuers != null);
        paymentMethods.Items.Should().Contain(x => x.Pricing != null && x.Pricing.Any(y => y.Fixed.Value > 0));
    }

    [DefaultRetryTheory]
    [InlineData("JPY", 249)]
    [InlineData("ISK", 50)]
    [InlineData("EUR", 50.25)]
    public async Task GetPaymentMethodListAsync_WithVariousCurrencies_ReturnsAvailablePaymentMethods(string currency, decimal value) {
        // When: Retrieving the payment methods for a currency and amount
        var amount = new Amount(currency, value);
        var paymentMethods = await _paymentMethodClient.GetPaymentMethodListAsync(amount: amount);

        // Then: We should have multiple payment methods
        paymentMethods.Count.Should().BeGreaterThan(0);
    }

    public void Dispose()
    {
        _paymentMethodClient?.Dispose();
    }
}
