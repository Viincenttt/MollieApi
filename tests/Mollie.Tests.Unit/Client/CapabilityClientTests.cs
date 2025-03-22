using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Capability;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class CapabilityClientTests {

    [Fact]
    public async Task GetCapabilityListAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get,$"{BaseMollieClient.ApiEndPoint}capabilities")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", DefaultListCapabilitiesResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var capabilityClient = new CapabilityClient("access_1234", httpClient);

        // Act
        var result = await capabilityClient.GetCapabilitiesListAsync();

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        result.Count.ShouldBe(1);
        var capability = result.Items[0];
        capability.Resource.ShouldBe("capability");
        //capability.Id.ShouldBe("payments"); // TODO: Figure out if this is name or id
        capability.Status.ShouldBe(CapabilityStatus.Pending);
        capability.StatusReason.ShouldBe(CapabilityStatusReason.OnboardingInformtionNeeded);
        capability.Requirements.Count().ShouldBe(2);
        var requirement1 = capability.Requirements.First();
        requirement1.Id.ShouldBe("legal-representatives");
        requirement1.DueDate.ShouldBeNull();
        requirement1.Status.ShouldBe(CapabilityRequirementStatus.Requested);
        requirement1.Links.ShouldNotBeNull();
        requirement1.Links.Dashboard.Href.ShouldBe("https://my.mollie.com/dashboard/");
        requirement1.Links.Dashboard.Type.ShouldBe("text/html");
        var requirement2 = capability.Requirements.Skip(1).First();
        requirement2.Id.ShouldBe("bank-account");
        requirement2.DueDate.ShouldBe(new DateTime(2024, 5, 14, 1, 29, 9, DateTimeKind.Utc));
        requirement2.Status.ShouldBe(CapabilityRequirementStatus.PastDue);
        requirement2.Links.ShouldNotBeNull();
        requirement2.Links.Dashboard.Href.ShouldBe("https://my.mollie.com/dashboard/");
        requirement2.Links.Dashboard.Type.ShouldBe("text/html");
    }

    private const string DefaultListCapabilitiesResponse = @"{
  ""count"": 1,
  ""_embedded"": {
    ""capabilities"": [
      {
        ""resource"": ""capability"",
        ""id"": ""payments"",
        ""status"": ""pending"",
        ""statusReason"": ""onboarding-information-needed"",
        ""requirements"": [
          {
            ""id"": ""legal-representatives"",
            ""dueDate"": null,
            ""status"": ""requested"",
            ""_links"": {
              ""dashboard"": {
                ""href"": ""https://my.mollie.com/dashboard/"",
                ""type"": ""text/html""
              }
            }
          },
          {
            ""id"": ""bank-account"",
            ""dueDate"": ""2024-05-14T01:29:09Z"",
            ""status"": ""past-due"",
            ""_links"": {
              ""dashboard"": {
                ""href"": ""https://my.mollie.com/dashboard/"",
                ""type"": ""text/html""
              }
            }
          }
        ]
      }
    ]
  },
  ""_links"": {
    ""documentation"": {
      ""href"": ""https://docs.mollie.com/reference/list-capabilities"",
      ""type"": ""text/html""
    }
  }
}";
}
