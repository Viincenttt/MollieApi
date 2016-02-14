using System;
using System.Threading.Tasks;
using Mollie.Api.Models.Issuer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Refund;

namespace Mollie.Api.Client {
    public class MollieApi {
        private readonly MollieHttpClient _httpClient;

        public MollieApi(string apiKey) {
            this.EnsureValidApiKeyFormat(apiKey);
            this._httpClient = new MollieHttpClient(apiKey);
        }

        public async Task<PaymentResponse> CreatePayment(PaymentRequest paymentRequest) {
            return await this._httpClient.Post<PaymentResponse>("payments", paymentRequest);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentList(int? offset = null, int? count = null) {
            return await this._httpClient.GetList<PaymentResponse>("payments", offset, count);
        }

        public async Task<PaymentResponse> GetPayment(string paymentId) {
            return await this._httpClient.Get<PaymentResponse>($"payments/{paymentId}");
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodList(int? offset = null, int? count = null) {
            return await this._httpClient.GetList<PaymentMethodResponse>("methods", offset, count);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethod(PaymentMethod paymentMethod) {
            return await this._httpClient.Get<PaymentMethodResponse>($"methods/{paymentMethod.ToString().ToLower()}");
        }

        public async Task<ListResponse<IssuerResponse>> GetIssuerList(int? offset = null, int? count = null) {
            return await this._httpClient.GetList<IssuerResponse>("issuers", offset, count);
        }

        public async Task<IssuerResponse> GetIssuer(string issuerId) {
            return await this._httpClient.Get<IssuerResponse>($"issuers/{issuerId}");
        }

        public async Task<RefundResponse> CreateRefund(string paymentId, decimal? amount = null) {
            return await this._httpClient.Post<RefundResponse>($"payments/{paymentId}/refunds", new { amount = amount });
        }

        public async Task<ListResponse<RefundResponse>> GetRefundList(string paymentId, int? offset = null, int? count = null) {
            return await this._httpClient.GetList<RefundResponse>($"payments/{paymentId}/refunds", offset, count);
        }

        public async Task<RefundResponse> GetRefund(string paymentId, string refundId) {
            return await this._httpClient.Get<RefundResponse>($"payments/{paymentId}/refunds/{refundId}");
        }

        public async Task CancelRefund(string paymentId, string refundId) {
            await this._httpClient.Delete($"payments/{paymentId}/refunds/{refundId}");
        }

        /// <summary>
        /// Make sure we are given a valid apikey. Valid api keys start with "test_" or "live_".
        /// </summary>
        private void EnsureValidApiKeyFormat(string apiKey) {
            if (apiKey == null || (!apiKey.StartsWith("test_") && !apiKey.StartsWith("live_"))) {
                throw new ArgumentException("Invalid API key detected! Api keys must start with \"test_\" or \"live_\".");
            }
        }
    }
}
