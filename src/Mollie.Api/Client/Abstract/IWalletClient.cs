using System.Threading.Tasks;
using Mollie.Api.Models.Wallet.Request;
using Mollie.Api.Models.Wallet.Response;

namespace Mollie.Api.Client.Abstract {
    public interface IWalletClient {
        Task<ApplePayPaymentSessionResponse> RequestApplePayPaymentSessionAsync(ApplePayPaymentSessionRequest request);
    }
}