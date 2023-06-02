using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api {
    public class ApiExceptionTests : BaseMollieApiTestClass {
        [Fact]
        //[RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task ShouldThrowMollieApiExceptionWhenInvalidParametersAreGiven() {
            // If: we create a payment request with invalid parameters
            IPaymentClient paymentClient = new PaymentClient(this.ApiKey);
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = null,
                RedirectUrl = null
            };
            
            // Then: Send the payment request to the Mollie Api, this should throw a mollie api exception
            MollieApiException apiException = await Assert.ThrowsAsync<MollieApiException>(() => paymentClient.CreatePaymentAsync(paymentRequest));
            apiException.Should().NotBeNull();
            apiException.Details.Should().NotBeNull();
            apiException.Details.Status.Should().Be(422);
            apiException.Details.Title.Should().Be("Unprocessable Entity");
            apiException.Details.Detail.Should().Be("The description is invalid");
        }
    }
}
