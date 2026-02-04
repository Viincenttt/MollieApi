using System;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Session.Request;
using Mollie.Api.Models.Session.Response;
using Mollie.Tests.Integration.Framework;
using System.Collections.Generic;
using Shouldly;
using Mollie.Api.Models.Order.Request;
using Xunit;
using Mollie.Api.Models.Payment;

namespace Mollie.Tests.Integration.Api;

public class SessionTests : BaseMollieApiTestClass, IDisposable {
    private readonly ISessionClient _sessionClient;

    public SessionTests(
        ISessionClient sessionClient,
        ICustomerClient customerClient,
        IMandateClient mandateClient,
        ITerminalClient terminalClient,
        ICaptureClient captureClient) {
        _sessionClient = sessionClient;
    }

    [Fact]
    public async Task CanCreateDefaultSessionWithOnlyRequiredFields() {
        // Given: we create a session request with only the required parameters
        var sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the session request to Mollie
        SessionResponse result = await _sessionClient.CreateSessionAsync(sessionRequest);

        // Then: Make sure we get a valid response
        result.ShouldNotBeNull();
        result.Amount.ShouldBe(sessionRequest.Amount);
        result.Description.ShouldBe(sessionRequest.Description);
        result.RedirectUrl.ShouldBe(sessionRequest.RedirectUrl);
    }

    [Fact]
    public async Task CanCreateDefaultSessionWithCustomIdempotencyKey() {
        // Given: we create a session request with only the required parameters
        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the session request to Mollie
        using (_sessionClient.WithIdempotencyKey("my-idempotency-key"))
        {
            SessionResponse firstAttempt = await _sessionClient.CreateSessionAsync(sessionRequest);
            SessionResponse secondAttempt = await _sessionClient.CreateSessionAsync(sessionRequest);

            // Then: Make sure the responses have the same session Id
            firstAttempt.Id.ShouldBe(secondAttempt.Id);
        }
    }

    [Fact]
    public async Task CanCreateSessionAndRetrieveIt() {
        // When: we create a new session request
        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the session request to Mollie and attempt to retrieve it
        SessionResponse sessionResponse = await _sessionClient.CreateSessionAsync(sessionRequest);
        SessionResponse result = await _sessionClient.GetSessionAsync(sessionResponse.Id);

        // Then
        result.ShouldNotBeNull();
        result.Id.ShouldBe(sessionResponse.Id);
        result.Amount.ShouldBe(sessionRequest.Amount);
        result.Description.ShouldBe(sessionRequest.Description);
        result.RedirectUrl.ShouldBe(sessionRequest.RedirectUrl);
    }

    [Fact]
    public async Task CanCreateSessionWithMetaData() {
        // When: We create a session with meta data
        string metadata = "this is my metadata";
        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Metadata = metadata
        };

        // When: We send the session request to Mollie
        SessionResponse result = await _sessionClient.CreateSessionAsync(sessionRequest);

        // Then: Make sure we get the same json result as metadata
        result.Metadata.ShouldBe(metadata);
    }

    [Fact]
    public async Task CanCreateSessionWithJsonMetaData() {
        // When: We create a session with meta data
        string json = "{\"order_id\":\"4.40\"}";
        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Metadata = json
        };

        // When: We send the session request to Mollie
        SessionResponse result = await _sessionClient.CreateSessionAsync(sessionRequest);

        // Then: Make sure we get the same json result as metadata
        IsJsonResultEqual(result.Metadata, json).ShouldBeTrue();
    }

    [Fact]
    public async Task CanCreateSessionWithCustomMetaDataClass() {
        // When: We create a session with meta data
        CustomMetadataClass metadataRequest = new CustomMetadataClass() {
            OrderId = 1,
            Description = "Custom description"
        };

        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
        };
        sessionRequest.SetMetadata(metadataRequest);

        // When: We send the session request to Mollie
        SessionResponse result = await _sessionClient.CreateSessionAsync(sessionRequest);
        CustomMetadataClass? metadataResponse = result.GetMetadata<CustomMetadataClass>();

        // Then: Make sure we get the same json result as metadata
        metadataResponse.ShouldNotBeNull();
        metadataResponse.OrderId.ShouldBe(metadataRequest.OrderId);
        metadataResponse.Description.ShouldBe(metadataRequest.Description);
    }

    [Fact]
    public async Task CanCreateSessionWithLines() {
        // Arrange
        var address = new PaymentAddressDetails {
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
        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, 90m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Lines = new List<PaymentLine>() {
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
            },
            ShippingAddress = address,
            BillingAddress = address
        };

        // Act
        SessionResponse result = await _sessionClient.CreateSessionAsync(sessionRequest);

        // Assert
        result.Lines.ShouldBeEquivalentTo(sessionRequest.Lines);
        result.BillingAddress.ShouldBeEquivalentTo(sessionRequest.BillingAddress);
        result.ShippingAddress.ShouldBeEquivalentTo(sessionRequest.ShippingAddress);
    }

    [Fact]
    public async Task CanCreateSessionWithDecimalAmountAndRetrieveIt() {
        // When: we create a new session request
        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, 100.1235m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the session request to Mollie and attempt to retrieve it
        SessionResponse sessionResponse = await _sessionClient.CreateSessionAsync(sessionRequest);
        SessionResponse result = await _sessionClient.GetSessionAsync(sessionResponse.Id);

        // Then
        result.ShouldNotBeNull();
        result.Id.ShouldBe(sessionResponse.Id);
        result.Amount.ShouldBe(sessionRequest.Amount);
        result.Description.ShouldBe(sessionRequest.Description);
        result.RedirectUrl.ShouldBe(sessionRequest.RedirectUrl);
    }

    [Fact]
    public async Task CanCreateSessionWithImplicitAmountCastAndRetrieveIt() {
        var initialAmount = 100.75m;

        // When: we create a new session request
        SessionRequest sessionRequest = new SessionRequest() {
            Amount = new Amount(Currency.EUR, initialAmount),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the session request to Mollie and attempt to retrieve it
        SessionResponse sessionResponse = await _sessionClient.CreateSessionAsync(sessionRequest);
        SessionResponse result = await _sessionClient.GetSessionAsync(sessionResponse.Id);

        decimal responseAmount = sessionResponse.Amount; // Implicit cast
        decimal resultAmount = result.Amount; // Implicit cast

        // Then
        result.ShouldNotBeNull();
        result.Id.ShouldBe(sessionResponse.Id);
        result.Amount.ShouldBe(sessionRequest.Amount);
        result.Description.ShouldBe(sessionRequest.Description);
        result.RedirectUrl.ShouldBe(sessionRequest.RedirectUrl);
        resultAmount.ShouldBe(responseAmount);
        resultAmount.ShouldBe(initialAmount);
    }

    public void Dispose()
    {
        _sessionClient.Dispose();
    }
}
