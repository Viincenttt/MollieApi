using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Framework.Authentication;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Options;
using RichardSzalay.MockHttp;
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
        const string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}payments";
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
        const string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}payments";
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

    [Fact]
    public async Task CustomUserAgentIsSetInOptions_UserAgentIsAppendedToDefaultUserAgent() {
        // Arrange
        const string customUserAgent = "my-user-agent";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get,$"{BaseMollieClient.DefaultBaseApiEndPoint}methods")
            .With(request => {
                var userAgent = request.Headers.UserAgent.ToArray();
                userAgent.ShouldNotBeNull();
                userAgent.Length.ShouldBe(2);
                userAgent[0].Product!.Name.ShouldStartWith("Mollie.Api.NET");
                userAgent[1].Product!.Name.ShouldBe(customUserAgent);

                return true;
            })
            .Respond("application/json", DefaultPaymentMethodJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var mollieClientOptions = new MollieClientOptions {
            CustomUserAgent = customUserAgent,
            ApiKey = "api-key"
        };
        var secretManager = new DefaultMollieSecretManager(mollieClientOptions.ApiKey);
        using var paymentMethodClient = new PaymentMethodClient(mollieClientOptions, secretManager, httpClient);

        // Act
        await paymentMethodClient.GetPaymentMethodListAsync();

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task NoCustomUserAgentIsSetInOptions_UserAgentIsAppendedToDefaultUserAgent() {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get,$"{BaseMollieClient.DefaultBaseApiEndPoint}methods")
            .With(request => {
                var userAgent = request.Headers.UserAgent.ToArray();
                userAgent.ShouldNotBeNull();
                userAgent.Length.ShouldBe(1);
                userAgent[0].Product!.Name.ShouldStartWith("Mollie.Api.NET");

                return true;
            })
            .Respond("application/json", DefaultPaymentMethodJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var mollieClientOptions = new MollieClientOptions {
            CustomUserAgent = null,
            ApiKey = "api-key"
        };
        var secretManager = new DefaultMollieSecretManager(mollieClientOptions.ApiKey);
        using var paymentMethodClient = new PaymentMethodClient(mollieClientOptions, secretManager, httpClient);

        // Act
        await paymentMethodClient.GetPaymentMethodListAsync();

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task CustomApiBaseUrlIsSetInOptions_RequestsAreRoutedToCustomApiUrl() {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        var mollieClientOptions = new MollieClientOptions {
            ApiKey = "api-key",
            ApiBaseUrl = "https://custom-api-base.mollie.com/v2/"
        };
        mockHttp.Expect(HttpMethod.Get,$"{mollieClientOptions.ApiBaseUrl}methods")
            .Respond("application/json", DefaultPaymentMethodJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var secretManager = new DefaultMollieSecretManager(mollieClientOptions.ApiKey);
        using var paymentMethodClient = new PaymentMethodClient(mollieClientOptions, secretManager, httpClient);

        // Act
        await paymentMethodClient.GetPaymentMethodListAsync();

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    private const string DefaultPaymentMethodJsonResponse = @"{
        ""count"": 13,
        ""_embedded"": {
            ""methods"": [
                {
                     ""resource"": ""method"",
                     ""id"": ""ideal"",
                     ""description"": ""iDEAL"",
                     ""minimumAmount"": {
                        ""value"": ""0.01"",
                        ""currency"": ""EUR""
                     },
                     ""maximumAmount"": {
                        ""value"": ""50000.00"",
                        ""currency"": ""EUR""
                     },
                     ""image"": {
                        ""size1x"": ""https://mollie.com/external/icons/payment-methods/ideal.png"",
                        ""size2x"": ""https://mollie.com/external/icons/payment-methods/ideal%402x.png"",
                        ""svg"": ""https://mollie.com/external/icons/payment-methods/ideal.svg""
                    },
                    ""status"": ""activated""
                }
            ]
        }
    }";
}
