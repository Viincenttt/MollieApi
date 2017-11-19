namespace Mollie.Api.Models.Connect
{
	public class TokenResponse
	{
		/// <summary>
		/// The access token, with which you will be able to access the Mollie API on the merchant's behalf.
		/// </summary>
		public string AccessToken { get; set; }

		/// <summary>
		/// The refresh token, with which you will be able to retrieve new access tokens on this endpoint. Please note that the refresh token does not expire.
		/// </summary>
		public string RefreshToken { get; set; }

		/// <summary>
		/// The number of seconds left before the access token expires. Be sure to renew your access token before this reaches zero.
		/// </summary>
		public int ExpiresIn { get; set; }

		/// <summary>
		/// As per OAuth standards, the provided access token can only be used with bearer authentication.
		/// Possible values: bearer
		/// </summary>
		public string TokenType { get; set; }

		/// <summary>
		/// A space separated list of permissions. Please refer to OAuth: Permissions for the full permission list.
		/// </summary>
		public string Scope { get; set; }
	}
}