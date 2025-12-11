using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Options;

namespace Mollie.Api.Client;

public class BalanceTransferClient : BaseMollieClient, IBalanceTransferClient {
    public BalanceTransferClient(string oauthAccessToken, HttpClient? httpClient = null)
        : base(oauthAccessToken, httpClient)
    {
    }

    [ActivatorUtilitiesConstructor]
    public BalanceTransferClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient)
    {
    }
}
