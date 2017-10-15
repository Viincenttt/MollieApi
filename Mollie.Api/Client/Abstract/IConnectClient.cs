using Mollie.Api.Models.Connect;

namespace Mollie.Api.Client.Abstract
{
	public interface IConnectClient
	{
		string GetAuthorizationUrl(AuthorizeRequest authorizeRequest);

		TokenResponse GetAccessToken(TokenRequest tokenRequest);
	}
}