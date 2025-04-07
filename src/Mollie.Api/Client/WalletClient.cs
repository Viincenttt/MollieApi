using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Wallet.Request;
using Mollie.Api.Models.Wallet.Response;

namespace Mollie.Api.Client {
    public class WalletClient : BaseMollieClient, IWalletClient {
        public WalletClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public WalletClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null) : base(mollieSecretManager, httpClient) {
        }

        public async Task<ApplePayPaymentSessionResponse> RequestApplePayPaymentSessionAsync(
            ApplePayPaymentSessionRequest request, CancellationToken cancellationToken = default) {
            return await PostAsync<ApplePayPaymentSessionResponse>(
                "wallets/applepay/sessions", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
