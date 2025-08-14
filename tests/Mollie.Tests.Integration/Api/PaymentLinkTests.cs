using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class PaymentLinkTests : BaseMollieApiTestClass, IDisposable {
    private readonly IPaymentLinkClient _paymentLinkClient;

    public PaymentLinkTests(IPaymentLinkClient paymentLinkClient) {
        _paymentLinkClient = paymentLinkClient;
    }

    [Fact]
    public async Task CanRetrievePaymentLinkList() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentLinkResponse> response = await _paymentLinkClient.GetPaymentLinkListAsync();

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanCreatePaymentLinkAndRetrieveIt() {
        // Given: We create a new payment link
        var address = CreateAddress();
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            Reusable = true,
            ExpiresAt = DateTime.Now.AddDays(1),
            BillingAddress = address,
            ShippingAddress = address
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.ShouldBe(paymentLinkRequest.Amount);
            response.MinimumAmount.ShouldBeNull();
            response.ExpiresAt.ShouldBe(expiresAtWithoutMs);
            response.Description.ShouldBe(paymentLinkRequest.Description);
            response.RedirectUrl.ShouldBe(paymentLinkRequest.RedirectUrl);
            response.Archived.ShouldBeFalse();
            response.Reusable.ShouldBe(paymentLinkRequest.Reusable);
            response.BillingAddress.ShouldBe(paymentLinkRequest.BillingAddress);
            response.ShippingAddress.ShouldBe(paymentLinkRequest.ShippingAddress);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }


    [Fact]
    public async Task CanCreatePaymentLinkWithMinimumAmount() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            MinimumAmount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            Reusable = true,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.ShouldBeNull();
            response.MinimumAmount.ShouldBe(paymentLinkRequest.MinimumAmount);
            response.ExpiresAt.ShouldBe(expiresAtWithoutMs);
            response.Description.ShouldBe(paymentLinkRequest.Description);
            response.RedirectUrl.ShouldBe(paymentLinkRequest.RedirectUrl);
            response.Archived.ShouldBeFalse();
            response.Reusable.ShouldBe(paymentLinkRequest.Reusable);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }

    [Fact]
    public async Task CanCreatePaymentLinkForRecurringPayments() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            Reusable = true,
            ExpiresAt = DateTime.Now.AddDays(1),
            SequenceType = SequenceType.First
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.ShouldBe(paymentLinkRequest.Amount);
            response.ExpiresAt.ShouldBe(expiresAtWithoutMs);
            response.Description.ShouldBe(paymentLinkRequest.Description);
            response.RedirectUrl.ShouldBe(paymentLinkRequest.RedirectUrl);
            response.Archived.ShouldBeFalse();
            response.Reusable.ShouldBe(paymentLinkRequest.Reusable);
            response.SequenceType.ShouldBe(paymentLinkRequest.SequenceType);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }

    [Fact]
    public async Task CanCreatePaymentLinkWithLines() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 90m),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            Reusable = false,
            ExpiresAt = DateTime.Now.AddDays(1),
            Lines = new List<PaymentLine> {
                new() {
                    Type = OrderLineDetailsType.Digital,
                    Description = "Star wars lego",
                    Quantity = 1,
                    QuantityUnit = "pcs",
                    UnitPrice = new Amount(Currency.EUR, 100m),
                    TotalAmount = new Amount(Currency.EUR, 90m),
                    DiscountAmount = new Amount(Currency.EUR, 10m),
                    ProductUrl = "http://www.lego.com/starwars",
                    ImageUrl = "http://www.lego.com/starwars.jpg",
                    Sku = "my-sku",
                    VatAmount = new Amount(Currency.EUR, 15.62m),
                    VatRate = "21.00"
                }
            }
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.ShouldBe(paymentLinkRequest.Amount);
            response.ExpiresAt.ShouldBe(expiresAtWithoutMs);
            response.Description.ShouldBe(paymentLinkRequest.Description);
            response.RedirectUrl.ShouldBe(paymentLinkRequest.RedirectUrl);
            response.Archived.ShouldBeFalse();
            response.Reusable.ShouldBe(paymentLinkRequest.Reusable);
            response.Lines.ShouldBe(paymentLinkRequest.Lines);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }

    [Fact]
    public async Task CanCreatePaymentLinkWithNullAmount() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount =  null,
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.ShouldBeNull();
            response.ExpiresAt.ShouldBe(expiresAtWithoutMs);
            response.Description.ShouldBe(paymentLinkRequest.Description);
            response.RedirectUrl.ShouldBe(paymentLinkRequest.RedirectUrl);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }


    [Fact]
    public async Task CanCreatePaymentLinkWithSpecificPaymentMethods() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1),
            AllowedMethods = [PaymentMethod.Ideal, PaymentMethod.CreditCard]
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.ShouldBe(paymentLinkRequest.Amount);
            response.ExpiresAt.ShouldBe(expiresAtWithoutMs);
            response.Description.ShouldBe(paymentLinkRequest.Description);
            response.RedirectUrl.ShouldBe(paymentLinkRequest.RedirectUrl);
            response.Archived.ShouldBeFalse();
            response.AllowedMethods.ShouldBe(paymentLinkRequest.AllowedMethods);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }

    [Fact]
    public async Task CanUpdatePaymentLink() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We update the payment link
        PaymentLinkUpdateRequest paymentLinkUpdateRequest = new() {
            Description = "Updated description",
            Archived = true,
            AllowedMethods = [PaymentMethod.CreditCard]
        };
        var updatedPaymentLinkResponse = await _paymentLinkClient.UpdatePaymentLinkAsync(
            createdPaymentLinkResponse.Id,
            paymentLinkUpdateRequest);

        // Then: We expect the payment link to be updated
        updatedPaymentLinkResponse.Description.ShouldBe(paymentLinkUpdateRequest.Description);
        updatedPaymentLinkResponse.Archived.ShouldBe(paymentLinkUpdateRequest.Archived);
        updatedPaymentLinkResponse.AllowedMethods.ShouldBe(paymentLinkUpdateRequest.AllowedMethods);
    }

    [Fact]
    public async Task CanDeletePaymentLink() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We delete the payment link
        await _paymentLinkClient.DeletePaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect the payment link to be updated
        MollieApiException exception = await Assert.ThrowsAsync<MollieApiException>(() =>
            _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id));
        exception.Details.Status.ShouldBe(404);
        exception.Details.Detail.ShouldBe("Payment link does not exists.");
    }

    [Fact]
    public async Task CanListPaymentLinkPayments() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We get the payment list of the payment link
        var result = await _paymentLinkClient.GetPaymentLinkPaymentListAsync(createdPaymentLinkResponse.Id);

        // Then: We expect the payment list to be returned
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(0);
    }

    private PaymentAddressDetails CreateAddress() {
        return new PaymentAddressDetails {
            Title = "Mr",
            GivenName = "John",
            FamilyName = "Doe",
            OrganizationName = "Mollie",
            StreetAndNumber = "Keizersgracht 126",
            Email = "johndoe@mollie.com",
            City = "Amsterdam",
            Country = "NL",
            Phone = "+31600000000",
            Region = "Zuid-Holland",
            PostalCode = "1015CW"
        };
    }

    public void Dispose()
    {
        _paymentLinkClient?.Dispose();
    }
}
