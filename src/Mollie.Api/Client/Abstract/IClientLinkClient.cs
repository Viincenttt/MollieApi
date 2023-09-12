using System.Collections.Generic;
using System.Threading.Tasks;
using Mollie.Api.Models.ClientLink.Request;
using Mollie.Api.Models.ClientLink.Response;

namespace Mollie.Api.Client.Abstract
{
    public interface IClientLinkClient
    {
        Task<ClientLinkResponse> CreateClientLinkAsync(ClientLinkRequest request);

        string GenerateClientLinkWithParameters(
            string clientLinkUrl,
            string state,
            List<string> scopes,
            bool forceApprovalPrompt = false);
    }
}