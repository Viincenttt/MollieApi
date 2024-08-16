using Mollie.Api.Framework.Authentication.Abstract;

namespace Mollie.Api.Framework.Authentication;

public class DefaultBearerTokenRetriever(string apiKey) : IBearerTokenRetriever {
    public string GetBearerToken() {
        return apiKey;
    }
}
