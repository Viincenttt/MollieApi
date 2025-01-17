using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class BaseMollieClientTests : BaseClientTests {
    [Fact]
    public async Task HttpResponseStatusCodeIsNotSuccesfull_ResponseBodyContainsMollieErrorDetails_MollieApiExceptionIsThrown() {

        // Arrange
        const string errorMessage = "A validation error occured";
        const int errorStatus = (int)HttpStatusCode.UnprocessableEntity;
        string responseBody = @$"{{
    ""_links"": {{
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/overview/handling-errors"",
            ""type"": ""text/html""
        }}
    }},
    ""detail"": ""{errorMessage}"",
    ""status"": {errorStatus},
    ""title"": ""Error""
}}";
        const string expectedUrl = $"{BaseMollieClient.ApiEndPoint}payments";
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            expectedUrl,
            responseBody,
            responseStatusCode: HttpStatusCode.UnprocessableEntity);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new("api-key", httpClient);
        PaymentRequest paymentRequest = new() {
            Amount = new Amount(Currency.EUR, 50m),
            Description = "description"
        };

        // Act
        var exception = await Assert.ThrowsAsync<MollieApiException>(() => paymentClient.CreatePaymentAsync(paymentRequest));

        // Assert
        exception.Details.Detail.ShouldBe(errorMessage);
        exception.Details.Status.ShouldBe(errorStatus);
    }

    [Fact]
    public async Task HttpResponseStatusCodeIsNotSuccesfull_ResponseBodyContainsHtml_MollieApiExceptionIsThrown() {
        // Arrange
        string responseBody = "<html><body>Whoops!</body></html>";
        const string expectedUrl = $"{BaseMollieClient.ApiEndPoint}payments";
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            expectedUrl,
            responseBody,
            responseContentType: MediaTypeNames.Text.Html,
            responseStatusCode: HttpStatusCode.UnprocessableEntity);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new("api-key", httpClient);
        PaymentRequest paymentRequest = new() {
            Amount = new Amount(Currency.EUR, 50m),
            Description = "description"
        };

        // Act
        var exception = await Assert.ThrowsAsync<MollieApiException>(() => paymentClient.CreatePaymentAsync(paymentRequest));

        // Assert
        exception.Details.Detail.ShouldBe(responseBody);
        exception.Details.Status.ShouldBe((int)HttpStatusCode.UnprocessableEntity);
    }
}
