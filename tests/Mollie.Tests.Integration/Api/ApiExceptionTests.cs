using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class ApiExceptionTests : BaseMollieApiTestClass {
    private readonly IPaymentClient _paymentClient;

    public ApiExceptionTests(IPaymentClient paymentClient) {
        _paymentClient = paymentClient;
    }

    [Fact]
    public async Task ShouldThrowMollieApiExceptionWhenInvalidParametersAreGiven() {
        // If: we create a payment request with invalid parameters
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = null,
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
}
