namespace Mollie.Api.Framework.Authentication.Abstract;

public interface IBearerTokenRetriever {
    public string GetBearerToken();
}
