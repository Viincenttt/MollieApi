namespace Mollie.Api.Framework.Authentication.Abstract;

public interface IMollieSecretManager {
    public string GetBearerToken();
}
