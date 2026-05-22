using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Payout.Request;
using Mollie.Api.Models.Payout.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client;

public class PayoutClient : BaseMollieClient, IPayoutClient {
    public PayoutClient(string apiKey, HttpClient? httpClient = null)
        : base(apiKey, httpClient)
    {
    }

    [ActivatorUtilitiesConstructor]
    public PayoutClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient)
    {
    }

    public async Task<PayoutResponse> CreatePayoutAsync(
        PayoutRequest request, CancellationToken cancellationToken = default) {
        return await PostAsync<PayoutResponse>("payouts", request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}

