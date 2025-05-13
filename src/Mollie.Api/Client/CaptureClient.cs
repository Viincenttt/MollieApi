using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Capture.Request;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client
{
    public class CaptureClient : BaseMollieClient, ICaptureClient {
        public CaptureClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public CaptureClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<CaptureResponse> GetCaptureAsync(
            string paymentId, string captureId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(captureId), captureId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<CaptureResponse>(
                    $"payments/{paymentId}/captures/{captureId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CaptureResponse> GetCaptureAsync(
            UrlObjectLink<CaptureResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetCaptureListAsync(
            string paymentId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<ListResponse<CaptureResponse>>(
                    $"payments/{paymentId}/captures{queryParameters.ToQueryString()}",
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetCaptureListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CaptureResponse> CreateCapture(string paymentId, CaptureRequest captureRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            return await PostAsync<CaptureResponse>(
                    $"payments/{paymentId}/captures", captureRequest,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}
