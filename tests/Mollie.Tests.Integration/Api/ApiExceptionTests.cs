using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Connect.Request;
using Mollie.Api.Models.Payment.Request;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class ApiExceptionTests : BaseMollieApiTestClass {
    private readonly IPaymentClient _paymentClient;
    private readonly IConnectClient _connectClient;

    public ApiExceptionTests(IPaymentClient paymentClient, IConnectClient connectClient) {
        _paymentClient = paymentClient;
        _connectClient = connectClient;
    }

    [Fact]
    public async Task CreatePayment_WithInvalidParameters_ShouldThrowMollieApiException() {
        // Given: we create a payment request with invalid parameters
        var paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = string.Empty,
            RedirectUrl = null
        };

        // Then: Send the payment request to the Mollie Api, this should throw a mollie api exception
        MollieApiException apiException = await Assert.ThrowsAsync<MollieApiException>(() => _paymentClient.CreatePaymentAsync(paymentRequest));
        apiException.ShouldNotBeNull();
        apiException.Details.ShouldNotBeNull();
        apiException.Details.Status.ShouldBe(422);
        apiException.Details.Title.ShouldBe("Unprocessable Entity");
        apiException.Details.Detail.ShouldBe("The description is invalid");
    }

    [Fact]
    public async Task RevokeTokenAsync_WithInvalidToken_ShouldThrowMollieApiException() {
        // Given
        var tokenRequest = new RevokeTokenRequest {
            Token = "token",
            TokenTypeHint = "hint"
        };

        // Then
        MollieApiException apiException = await Assert.ThrowsAsync<MollieApiException>(() => _connectClient.RevokeTokenAsync(tokenRequest));
        apiException.Details.Title.ShouldBe("invalid_request");
    }
}
