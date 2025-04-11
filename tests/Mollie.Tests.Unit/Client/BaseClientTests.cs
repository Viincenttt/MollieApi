using System.Net;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Net.Mime;

namespace Mollie.Tests.Unit.Client {
    public abstract class BaseClientTests {
        protected const string DefaultRedirectUrl = "https://www.mollie.com";

        protected MockHttpMessageHandler CreateMockHttpMessageHandler(
            HttpMethod httpMethod,
            string url,
            string response,
            string? expectedPartialContent = null,
            string responseContentType =  MediaTypeNames.Application.Json,
            HttpStatusCode responseStatusCode = HttpStatusCode.OK) {

            MockHttpMessageHandler mockHttp = new();
            MockedRequest mockedRequest = mockHttp.Expect(httpMethod, url)
                .Respond(responseStatusCode, responseContentType, response);

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
