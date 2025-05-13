using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Capability.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client;

public class CapabilityClient : BaseMollieClient, ICapabilityClient {
    public CapabilityClient(string oauthAccessToken, HttpClient? httpClient = null)
        : base(oauthAccessToken, httpClient)
    {
    }

    [ActivatorUtilitiesConstructor]
    public CapabilityClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient)
    {
    }

    public async Task<ListResponse<CapabilityResponse>> GetCapabilitiesListAsync(CancellationToken cancellationToken = default) {
        return await GetListAsync<ListResponse<CapabilityResponse>>(
                "capabilities", from: null, limit: null, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
