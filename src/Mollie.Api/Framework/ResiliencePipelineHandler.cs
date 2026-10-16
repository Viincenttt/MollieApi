using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Mollie.Api.Framework {
    /// <summary>
    /// Executes outgoing requests through a Polly resilience pipeline. Used instead of
    /// Microsoft.Extensions.Http.Resilience, which does not support .NET Framework.
    /// </summary>
    internal sealed class ResiliencePipelineHandler : DelegatingHandler {
        private readonly ResiliencePipeline<HttpResponseMessage> _pipeline;

        public ResiliencePipelineHandler(ResiliencePipeline<HttpResponseMessage> pipeline) {
            _pipeline = pipeline;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            return await _pipeline.ExecuteAsync(
                async token => await base.SendAsync(request, token).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }
    }
}
