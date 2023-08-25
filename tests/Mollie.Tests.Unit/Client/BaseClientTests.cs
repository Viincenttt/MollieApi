using RichardSzalay.MockHttp;
using System.Net.Http;

namespace Mollie.Tests.Unit.Client {
    public abstract class BaseClientTests {
        protected MockHttpMessageHandler CreateMockHttpMessageHandler(HttpMethod httpMethod, string url, string response, string expectedPartialContent = null) {
            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            MockedRequest mockedRequest = mockHttp.Expect(httpMethod, url)
                .Respond("application/json", response);

            if (!string.IsNullOrEmpty(expectedPartialContent)) {
                mockedRequest.WithPartialContent(expectedPartialContent);
            }

            return mockHttp;
        }
    }
}
