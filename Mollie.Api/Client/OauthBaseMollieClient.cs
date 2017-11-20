using System;

namespace Mollie.Api.Client
{
    public class OauthBaseMollieClient : BaseMollieClient
	{
		public OauthBaseMollieClient(string oauthAccessToken) : base(oauthAccessToken)
		{
			if (string.IsNullOrWhiteSpace(oauthAccessToken))
			{
				throw new ArgumentNullException(nameof(oauthAccessToken), "Mollie API key cannot be empty");
			}

			if (!oauthAccessToken.StartsWith("access_"))
			{
				throw new ArgumentException("The provided token isn't an oauth token.", nameof(oauthAccessToken));
			}
		}
	}
}