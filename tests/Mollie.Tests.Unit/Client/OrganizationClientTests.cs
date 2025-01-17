using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models.Organization;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class OrganizationClientTests : BaseClientTests
{
    [Fact]
    public async Task GetCurrentOrganizationAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}organizations/me")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultOrganizationResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var organizationsClient = new OrganizationClient("access_abcde", httpClient);

        // Act
        var result = await organizationsClient.GetCurrentOrganizationAsync();

        // Assert
        AssertDefaultOrganization(result);
    }

    [Fact]
    public async Task GetOrganizationAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        const string organizationId = "organization-id";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}organizations/{organizationId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultOrganizationResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var organizationsClient = new OrganizationClient("access_abcde", httpClient);

        // Act
        var result = await organizationsClient.GetOrganizationAsync(organizationId);

        // Assert
        AssertDefaultOrganization(result);
    }

    [Fact]
    public async Task GetOrganizationsListAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}organizations")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultOrganizationListResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var organizationsClient = new OrganizationClient("access_abcde", httpClient);

        // Act
        var result = await organizationsClient.GetOrganizationListAsync();

        // Assert
        result.Count.ShouldBe(2);
        result.Items.Count.ShouldBe(2);
    }

    private void AssertDefaultOrganization(OrganizationResponse response)
    {
        response.Resource.ShouldBe("organization");
        response.Id.ShouldBe("org_12345678");
        response.Name.ShouldBe("Mollie B.V.");
        response.Email.ShouldBe("info@mollie.com");
        response.Address.StreetAndNumber.ShouldBe("Keizersgracht 126");
        response.Address.PostalCode.ShouldBe("1015 CW");
        response.Address.City.ShouldBe("Amsterdam");
        response.Address.Country.ShouldBe("NL");
        response.RegistrationNumber.ShouldBe("30204462");
        response.VatNumber.ShouldBe("NL815839091B01");
        response.VatRegulation.ShouldBeNullOrEmpty();
        response.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/organizations/me");
        response.Links.Chargebacks.Href.ShouldBe("https://api.mollie.com/v2/chargebacks");
        response.Links.Customers.Href.ShouldBe("https://api.mollie.com/v2/customers");
        response.Links.Invoices.Href.ShouldBe("https://api.mollie.com/v2/invoices");
        response.Links.Payments.Href.ShouldBe("https://api.mollie.com/v2/payments");
        response.Links.Profiles.Href.ShouldBe("https://api.mollie.com/v2/profiles");
        response.Links.Refunds.Href.ShouldBe("https://api.mollie.com/v2/refunds");
        response.Links.Settlements.Href.ShouldBe("https://api.mollie.com/v2/settlements");
        response.Links.Dashboard.Href.ShouldBe("https://mollie.com/dashboard/org_12345678");
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/organizations-api/current-organization");
    }

    private const string defaultOrganizationListResponse = @$"
{{
    ""count"": ""2"",
    ""_embedded"": {{
        ""organizations"": [
            {defaultOrganizationResponse},
            {defaultOrganizationResponse}
        ]
    }}
}}";

    private const string defaultOrganizationResponse = @"{
     ""resource"": ""organization"",
     ""id"": ""org_12345678"",
     ""name"": ""Mollie B.V."",
     ""email"": ""info@mollie.com"",
     ""address"": {
        ""streetAndNumber"" : ""Keizersgracht 126"",
        ""postalCode"": ""1015 CW"",
         ""city"": ""Amsterdam"",
         ""country"": ""NL""
     },
     ""registrationNumber"": ""30204462"",
     ""vatNumber"": ""NL815839091B01"",
     ""_links"": {
         ""self"": {
             ""href"": ""https://api.mollie.com/v2/organizations/me"",
             ""type"": ""application/hal+json""
         },
         ""chargebacks"": {
             ""href"": ""https://api.mollie.com/v2/chargebacks"",
             ""type"": ""application/hal+json""
         },
         ""customers"": {
             ""href"": ""https://api.mollie.com/v2/customers"",
             ""type"": ""application/hal+json""
         },
         ""invoices"": {
             ""href"": ""https://api.mollie.com/v2/invoices"",
             ""type"": ""application/hal+json""
         },
         ""payments"": {
             ""href"": ""https://api.mollie.com/v2/payments"",
             ""type"": ""application/hal+json""
         },
         ""profiles"": {
             ""href"": ""https://api.mollie.com/v2/profiles"",
             ""type"": ""application/hal+json""
         },
         ""refunds"": {
             ""href"": ""https://api.mollie.com/v2/refunds"",
             ""type"": ""application/hal+json""
         },
         ""settlements"": {
             ""href"": ""https://api.mollie.com/v2/settlements"",
             ""type"": ""application/hal+json""
         },
         ""dashboard"": {
             ""href"": ""https://mollie.com/dashboard/org_12345678"",
             ""type"": ""text/html""
         },
         ""documentation"": {
             ""href"": ""https://docs.mollie.com/reference/v2/organizations-api/current-organization"",
             ""type"": ""text/html""
         }
     }
 }";
}
