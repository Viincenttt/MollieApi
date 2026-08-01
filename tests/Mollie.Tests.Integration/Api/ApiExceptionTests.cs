using System.Threading.Tasks;
using Shouldly;
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
    public async Task CreatePayment_WithInvalidParameters_ShouldReturnErrorResult() {
        // Given: we create a payment request with invalid parameters
        var paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = string.Empty,
            RedirectUrl = null
        };

        // Then: Send the payment request to the Mollie Api, this should return an error result
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        result.Success.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Status.ShouldBe(422);
        result.Error.Title.ShouldBe("Unprocessable Entity");
        result.Error.Detail.ShouldBe("The description is invalid");
    }

    [Fact]
    public async Task RevokeTokenAsync_WithInvalidToken_ShouldReturnErrorResult() {
        // Given
        var tokenRequest = new RevokeTokenRequest {
            Token = "token",
            TokenTypeHint = "hint"
        };

        // Then
        var result = await _connectClient.RevokeTokenAsync(tokenRequest);
        result.Success.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Title.ShouldBe("invalid_client");
    }
}
