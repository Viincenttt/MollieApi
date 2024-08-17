using Mollie.Api.Framework.Authentication.Abstract;
namespace Mollie.Api.Framework.Authentication;

public class DefaultMollieSecretManager(string apiKey) : IMollieSecretManager {
    public string GetBearerToken() => apiKey;
}
