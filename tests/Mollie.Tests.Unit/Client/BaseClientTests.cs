using RichardSzalay.MockHttp;
using System.Net.Http;

namespace Mollie.Tests.Unit.Client {
    public abstract class BaseClientTests {
        protected const string DefaultRedirectUrl = "https://www.mollie.com";

        protected MockHttpMessageHandler CreateMockHttpMessageHandler(HttpMethod httpMethod, string url, string response, string expectedPartialContent = null) {
            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            MockedRequest mockedRequest = mockHttp.Expect(httpMethod, url)
                .Respond("application/json", response);

            if (!string.IsNullOrEmpty(expectedPartialContent))
            {
                mockedRequest.With(x =>
                {
                    string expectedContent = RemoveWhiteSpaces(expectedPartialContent);
                    string content = RemoveWhiteSpaces(x.Content!.ReadAsStringAsync().Result);
                    return content.Contains(expectedContent);
                });
            }

            return mockHttp;
        }

        private string RemoveWhiteSpaces(string input)
        {
            return input
                .Replace(System.Environment.NewLine, string.Empty)
                .Replace(" ", string.Empty);
        }
    }
}
