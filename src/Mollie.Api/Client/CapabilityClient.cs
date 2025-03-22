using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Capability.Response;
using Mollie.Api.Models.List.Response;

namespace Mollie.Api.Client;

public class CapabilityClient : BaseMollieClient, ICapabilityClient {
    public CapabilityClient(string oauthAccessToken, HttpClient? httpClient = null)
        : base(oauthAccessToken, httpClient)
    {
    }

    public CapabilityClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(mollieSecretManager, httpClient)
    {
    }

    public async Task<ListResponse<CapabilityResponse>> GetCapabilitiesListAsync() {
        return await GetListAsync<ListResponse<CapabilityResponse>>($"clients", from: null, limit: null)
            .ConfigureAwait(false);
    }
}
