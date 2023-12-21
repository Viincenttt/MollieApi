using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Profile;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class ProfileClientTests : BaseClientTests
{
    [Fact]
    public async Task CreateProfileAsync_WithRequiredParameters_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        ProfileRequest profileRequest = new ProfileRequest() {
            Name = "My website name",
            Email = "info@mywebsite.com",
            Mode = Mode.Test,
            Phone = "+31208202070",
            Website = "https://www.mywebsite.com",
            BusinessCategory = "OTHER_MERCHANDISE"
        };
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}profiles")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultProfileJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var result = await profileClient.CreateProfileAsync(profileRequest);

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        AssertDefaultProfileResponse(result);
    }
    
    [Fact]
    public async Task GetProfileAsync_WithProfileId_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        const string profileId = "profile-id";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get,$"{BaseMollieClient.ApiEndPoint}profiles/{profileId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultProfileJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var result = await profileClient.GetProfileAsync(profileId);

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        AssertDefaultProfileResponse(result);
    }

    [Fact]
    public async Task GetCurrentProfileAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}profiles/me")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultProfileJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var result = await profileClient.GetCurrentProfileAsync();

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        AssertDefaultProfileResponse(result);
    }

    [Fact]
    public async Task UpdateProfileAsync_WithRequiredParameters_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        const string profileId = "profileId";
        ProfileRequest profileRequest = new ProfileRequest() {
            Name = "My website name",
            Email = "info@mywebsite.com",
            Mode = Mode.Test,
            Phone = "+31208202070",
            Website = "https://www.mywebsite.com",
            BusinessCategory = "OTHER_MERCHANDISE"
        };
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Patch, $"{BaseMollieClient.ApiEndPoint}profiles/{profileId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultProfileJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var result = await profileClient.UpdateProfileAsync(profileId, profileRequest);

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        AssertDefaultProfileResponse(result);
    }
    
    [Fact]
    public async Task UpdateProfileAsync_WithMissingProfileIdParameter_ThrowsArgumentException()
    {
        // Arrange
        const string profileId = "";
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => profileClient.UpdateProfileAsync(profileId, new ProfileRequest()));

        // Assert
        exception.Message.Should().Be($"Required URL argument '{nameof(profileId)}' is null or empty");
    }

    [Fact]
    public async Task EnablePaymentMethodAsync_ForCurrentProfile_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        const string paymentMethod = PaymentMethod.Ideal;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post,$"{BaseMollieClient.ApiEndPoint}profiles/me/methods/{paymentMethod}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultPaymentMethodResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var result = await profileClient.EnablePaymentMethodAsync(paymentMethod);

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        result.Resource.Should().Be("method");
        result.Id.Should().Be(paymentMethod);
    }
    
    [Fact]
    public async Task EnablePaymentMethodAsync_ForCurrentProfileWithMissingPaymentMethodParameter_ThrowsArgumentException()
    {
        // Arrange
        const string paymentMethod = PaymentMethod.Ideal;
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => profileClient.EnablePaymentMethodAsync(string.Empty));

        // Assert
        exception.Message.Should().Be($"Required URL argument '{nameof(paymentMethod)}' is null or empty");
    }
    
    [Fact]
    public async Task DisablePaymentMethodAsync_ForCurrentProfile_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        const string paymentMethod = PaymentMethod.Ideal;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Delete, $"{BaseMollieClient.ApiEndPoint}profiles/me/methods/{paymentMethod}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond(HttpStatusCode.NoContent);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        await profileClient.DisablePaymentMethodAsync(paymentMethod);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }
    
    [Fact]
    public async Task DisablePaymentMethodAsync_ForCurrentProfileWithMissingPaymentMethodParameter_ThrowsArgumentException()
    {
        // Arrange
        const string paymentMethod = PaymentMethod.Ideal;
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var profileClient = new ProfileClient("abcde", httpClient);
        
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => profileClient.DisablePaymentMethodAsync(string.Empty));

        // Assert
        exception.Message.Should().Be($"Required URL argument '{nameof(paymentMethod)}' is null or empty");
    }
    
    //[Fact]
    //public async Task 

    private void AssertDefaultProfileResponse(ProfileResponse result)
    {
        result.Resource.Should().Be("profile");
        result.Id.Should().Be("pfl_v9hTwCvYqw");
        result.Mode.Should().Be(Mode.Test);
        result.Name.Should().Be("My website name");
        result.Email.Should().Be("info@mywebsite.com");
        result.Website.Should().Be("https://www.mywebsite.com");
        result.Phone.Should().Be("+31208202070");
        result.BusinessCategory.Should().Be("OTHER_MERCHANDISE");
        result.Status.Should().Be(ProfileStatus.Unverified);
    }

    private const string defaultPaymentMethodResponse = @"{
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
         ""size1x"": ""https://www.mollie.com/external/icons/payment-methods/ideal.png"",
         ""size2x"": ""https://www.mollie.com/external/icons/payment-methods/ideal%402x.png"",
         ""svg"": ""https://www.mollie.com/external/icons/payment-methods/ideal.svg""
     },
     ""status"": ""activated"",
     ""_links"": {
         ""self"": {
             ""href"": ""https://api.mollie.com/v2/methods/ideal"",
             ""type"": ""application/hal+json""
         },
         ""documentation"": {
             ""href"": ""https://docs.mollie.com/reference/v2/profiles-api/enable-method"",
             ""type"": ""text/html""
         }
     }
 }";
    
    private const string defaultProfileJsonResponse = @"{
    ""resource"": ""profile"",
    ""id"": ""pfl_v9hTwCvYqw"",
    ""mode"": ""test"",
    ""name"": ""My website name"",
    ""website"": ""https://www.mywebsite.com"",
    ""email"": ""info@mywebsite.com"",
    ""phone"": ""+31208202070"",
    ""businessCategory"": ""OTHER_MERCHANDISE"",
    ""categoryCode"": 5399,
    ""status"": ""unverified"",
    ""createdAt"": ""2018-03-20T09:28:37+00:00"",
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/profiles/pfl_v9hTwCvYqw"",
            ""type"": ""application/hal+json""
        },
        ""dashboard"": {
            ""href"": ""https://www.mollie.com/dashboard/org_123456789/settings/profiles/pfl_v9hTwCvYqw"",
            ""type"": ""text/html""
        },
        ""chargebacks"": {
            ""href"": ""https://api.mollie.com/v2/chargebacks?profileId=pfl_v9hTwCvYqw"",
            ""type"": ""application/hal+json""
        },
        ""methods"": {
            ""href"": ""https://api.mollie.com/v2/methods?profileId=pfl_v9hTwCvYqw"",
            ""type"": ""application/hal+json""
        },
        ""payments"": {
            ""href"": ""https://api.mollie.com/v2/payments?profileId=pfl_v9hTwCvYqw"",
            ""type"": ""application/hal+json""
        },
        ""refunds"": {
            ""href"": ""https://api.mollie.com/v2/refunds?profileId=pfl_v9hTwCvYqw"",
            ""type"": ""application/hal+json""
        },
        ""checkoutPreviewUrl"": {
            ""href"": ""https://www.mollie.com/payscreen/preview/pfl_v9hTwCvYqw"",
            ""type"": ""text/html""
        },
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/profiles-api/create-profile"",
            ""type"": ""text/html""
        }
    }
}";
}