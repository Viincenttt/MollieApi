using System.Threading.Tasks;
using Mollie.Api.Models.Organization;

namespace Mollie.Api.Client.Abstract
{
	public interface IOrganizationsClient
	{
		Task<OrganizationResponse> GetOrganizationAsync(string organizationId);
	}
}