using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Wallet.Request;
using Mollie.Api.Models.Wallet.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class WalletClient : BaseMollieClient, IWalletClient {
        public WalletClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public WalletClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<ApplePayPaymentSessionResponse> RequestApplePayPaymentSessionAsync(
            ApplePayPaymentSessionRequest request, CancellationToken cancellationToken = default) {
            return await PostAsync<ApplePayPaymentSessionResponse>(
                "wallets/applepay/sessions", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
