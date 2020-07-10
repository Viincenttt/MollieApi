using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Onboarding.Request;
using Mollie.Api.Models.Onboarding.Response;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class OnboardingClientTests : BaseClientTests {
        public const string defaultName = "Mollie API Unit Test";
        public const string defaultStatus = OnboardingStatus.Completed;
        public const string canReceivePayments = "true";
        public const string canReceiveSettlements = "true";
        public const string defaultStreetAndNumber = "My address";

        public static readonly string defaultOnboardingStatusJsonResponse = $@"{{
    ""resource"": ""onboarding"",
    ""name"": ""{defaultName}"",
    ""signedUpAt"": ""2018-12-20T10:49:08+00:00"",
    ""status"": ""{defaultStatus}"",
    ""canReceivePayments"": {canReceivePayments},
    ""canReceiveSettlements"": {canReceiveSettlements},
}}";

        public static readonly string defaultOnboardingStatusJsonRequest = $@"{{
""organization"":{{""name"":""{defaultName}"",""address"":{{""streetAndNumber"":""{defaultStreetAndNumber}""}}}},
""profile"":{{""name"":""{defaultName}""}}
}}";

        [Test]
        public async Task GetOnboardingStatusAsync_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request the onboarding status
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}onboarding/me";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultOnboardingStatusJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OnboardingClient onboardingClient = new OnboardingClient("api-key", httpClient);

            // When: We make the request
            OnboardingStatusResponse onboardingResponse = await onboardingClient.GetOnboardingStatusAsync();

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(onboardingResponse);
            Assert.AreEqual(defaultName, onboardingResponse.Name);
            Assert.AreEqual(defaultStatus, onboardingResponse.Status);
            Assert.AreEqual(canReceivePayments, onboardingResponse.CanReceivePayments.ToString().ToLower());
            Assert.AreEqual(canReceiveSettlements, onboardingResponse.CanReceiveSettlements.ToString().ToLower());
        }

        [Test]
        public async Task SubmitOnboardingDataAsync_DefaultBehaviour_RequestIsParsed() {
            // Given: We submit an onboarding status request
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}onboarding/me";
            SubmitOnboardingDataRequest submitOnboardingDataRequest = new SubmitOnboardingDataRequest() {
                Organization = new OnboardingOrganizationRequest() {
                    Name = defaultName,
                    Address = new AddressObject() {
                        StreetAndNumber = defaultStreetAndNumber
                    }
                },
                Profile = new OnboardingProfileRequest() {
                    Name = defaultName
                }
            };
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, expectedUrl, string.Empty);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OnboardingClient onboardingClient = new OnboardingClient("api-key", httpClient);

            // When: We make the request
            await onboardingClient.SubmitOnboardingDataAsync(submitOnboardingDataRequest);

            // Then: There should be no outstanding requests
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}
