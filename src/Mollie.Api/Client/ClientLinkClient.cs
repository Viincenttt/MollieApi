using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.ClientLink.Request;
using Mollie.Api.Models.ClientLink.Response;

namespace Mollie.Api.Client {
    public class ClientLinkClient : OauthBaseMollieClient, IClientLinkClient
    {
        public ClientLinkClient(string oauthAccessToken, HttpClient httpClient = null) 
            : base(oauthAccessToken, httpClient) { }

        public async Task<ClientLinkResponse> CreateClientLinkAsync(ClientLinkRequest request)
        {
            return await this.PostAsync<ClientLinkResponse>($"client-links", request)
                .ConfigureAwait(false);
        }
    }
}