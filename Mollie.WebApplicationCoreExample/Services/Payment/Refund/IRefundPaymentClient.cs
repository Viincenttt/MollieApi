using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;

namespace Mollie.WebApplicationCoreExample.Services.Payment.Refund {
    public interface IRefundPaymentClient {
        Task Refund(string paymentId);
    }

    public class RefundPaymentClient : IRefundPaymentClient {
        private readonly IRefundClient _refundClient;
        private readonly IPaymentClient _paymentClient;

        public RefundPaymentClient(IRefundClient refundClient, IPaymentClient paymentClient) {
            this._refundClient = refundClient;
            this._paymentClient = paymentClient;
        }

        public async Task Refund(string paymentId) {
            PaymentResponse paymentToRefund = await this.GetPaymentDetails(paymentId);
            RefundRequest refundRequest = new RefundRequest() {
                Amount = new Amount(paymentToRefund.Amount.Currency, paymentToRefund.Amount.Value)
            };

            await this._refundClient.CreateRefundAsync(paymentId, refundRequest);
        }

        private async Task<PaymentResponse> GetPaymentDetails(string paymentId) {
            return await this._paymentClient.GetPaymentAsync(paymentId);
        }
    }
}