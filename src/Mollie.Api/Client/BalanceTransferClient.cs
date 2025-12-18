using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.BalanceTransfer.Request;
using Mollie.Api.Models.BalanceTransfer.Response;
using Mollie.Api.Models.List.Response;
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

    public async Task<BalanceTransferResponse> CreateBalanceTransferAsync(
        BalanceTransferRequest request, CancellationToken cancellationToken = default) {
        return await PostAsync<BalanceTransferResponse>(
                "connect/balance-transfers", request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ListResponse<BalanceTransferResponse>> GetBalanceTransferListAsync(
        string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
        var queryParameters = BuildQueryParameters(testmode);
        return await GetListAsync<ListResponse<BalanceTransferResponse>>(
                "connect/balance-transfers", from, limit, queryParameters, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<BalanceTransferResponse> GetBalanceTransferAsync(
        string balanceTransferId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(balanceTransferId), balanceTransferId);
        var queryParameters = BuildQueryParameters(testmode);
        return await GetAsync<BalanceTransferResponse>(
                $"connect//balance-transfers/{balanceTransferId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
        var result = new Dictionary<string, string>();
        result.AddValueIfTrue("testmode", testmode);
        return result;
    }
}
