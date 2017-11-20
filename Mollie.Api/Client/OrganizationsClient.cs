using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Organization;

namespace Mollie.Api.Client
{
	public class OrganizationsClient : OauthBaseMollieClient, IOrganizationsClient
	{
		public OrganizationsClient(string oauthAccessToken) : base(oauthAccessToken) { }

		public async Task<OrganizationResponse> GetOrganizationAsync(string id)
		{
			return await this.GetAsync<OrganizationResponse>($"organizations/{id}").ConfigureAwait(false);
		}
	}
}