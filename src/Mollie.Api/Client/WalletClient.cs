using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Wallet.Request;
using Mollie.Api.Models.Wallet.Response;

namespace Mollie.Api.Client {
    public class WalletClient : BaseMollieClient, IWalletClient {
        public WalletClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<ApplePayPaymentSessionResponse> RequestApplePayPaymentSessionAsync(ApplePayPaymentSessionRequest request) {
            return await PostAsync<ApplePayPaymentSessionResponse>("wallets/applepay/sessions", request).ConfigureAwait(false);
        }
    }
}
