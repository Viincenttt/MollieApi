using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.Wallet.Request;
using Mollie.Api.Models.Wallet.Response;

namespace Mollie.Api.Client.Abstract {
    public interface IWalletClient : IBaseMollieClient {
        Task<ApplePayPaymentSessionResponse> RequestApplePayPaymentSessionAsync(ApplePayPaymentSessionRequest request, CancellationToken cancellationToken = default);
    }
}
