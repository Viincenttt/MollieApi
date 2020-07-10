using Mollie.Api.Client;
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

        public static readonly string defaultOnboardingStatusJsonResponse = $@"{{
    ""resource"": ""onboarding"",
    ""name"": ""{defaultName}"",
    ""signedUpAt"": ""2018-12-20T10:49:08+00:00"",
    ""status"": ""{defaultStatus}"",
    ""canReceivePayments"": {canReceivePayments},
    ""canReceiveSettlements"": {canReceiveSettlements},
}}";

        [Test]
        public async Task GetOnboardingStatusAsync_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a capture with a payment id and capture id
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
    }
}
