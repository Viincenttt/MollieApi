using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
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

    public async Task<ListResponse<PayoutResponse>> GetPayoutListAsync(
        string? balanceId = null, string? from = null, int? limit = null,
        SortDirection? sort = null, bool testmode = false, CancellationToken cancellationToken = default) {
        var queryParameters = BuildQueryParameters(testmode: testmode, sort: sort);
        queryParameters.AddValueIfNotNullOrEmpty(nameof(balanceId), balanceId);
        return await GetListAsync<ListResponse<PayoutResponse>>(
                "payouts", from, limit, queryParameters, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<PayoutResponse> GetPayoutAsync(
        string payoutId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(payoutId), payoutId);
        var queryParameters = BuildQueryParameters(testmode: testmode);
        return await GetAsync<PayoutResponse>(
                $"payouts/{payoutId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<PayoutResponse> CancelPayoutAsync(
        string payoutId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(payoutId), payoutId);
        var data = CreateTestmodeModel(testmode);
        return await DeleteAsync<PayoutResponse>($"payouts/{payoutId}", data, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
