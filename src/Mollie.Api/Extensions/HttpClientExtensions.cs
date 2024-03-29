using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mollie.Api.Extensions {
    internal static class HttpClientExtensions {
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content) {
            return client.PatchAsync(CreateUri(requestUri), content);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content) {
            return client.PatchAsync(requestUri, content, CancellationToken.None);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken) {
            return client.PatchAsync(CreateUri(requestUri), content, cancellationToken);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken) {
            return client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content }, cancellationToken);
        }

        private static Uri CreateUri(string uri) {
            return string.IsNullOrEmpty(uri) ? null : new Uri(uri, UriKind.RelativeOrAbsolute);
        }
    }
}