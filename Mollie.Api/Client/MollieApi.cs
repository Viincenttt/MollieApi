using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
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
            string paymentListUri = this.GetRelativeListUri("payments", offset, count);
            return await this._httpClient.Get<ListResponse<PaymentResponse>>(paymentListUri);
        }

        public async Task<PaymentResponse> GetPayment(string paymentId) {
            return await this._httpClient.Get<PaymentResponse>($"payments/{paymentId}");
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodList(int? offset = null, int? count = null) {
            string paymentMethodListUri = this.GetRelativeListUri("methods", offset, count);
            return await this._httpClient.Get<ListResponse<PaymentMethodResponse>>(paymentMethodListUri);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethod(PaymentMethod paymentMethod) {
            return await this._httpClient.Get<PaymentMethodResponse>($"methods/{paymentMethod.ToString().ToLower()}");
        }

        public async Task<ListResponse<IssuerResponse>> GetIssuerList(int? offset = null, int? count = null) {
            string issuerListUri = this.GetRelativeListUri("issuers", offset, count);
            return await this._httpClient.Get<ListResponse<IssuerResponse>>(issuerListUri);
        }

        public async Task<IssuerResponse> GetIssuer(string issuerId) {
            return await this._httpClient.Get<IssuerResponse>($"issuers/{issuerId}");
        }

        public async Task<RefundResponse> CreateRefund(string paymentId, decimal? amount = null) {
            return await this._httpClient.Post<RefundResponse>($"payments/{paymentId}/refunds", new { amount = amount });
        }

        public async Task<ListResponse<RefundResponse>> GetRefundList(string paymentId, int? offset = null, int? count = null) {
            string refundListUri = this.GetRelativeListUri($"payments/{paymentId}/refunds", offset, count);
            return await this._httpClient.Get<ListResponse<RefundResponse>>(refundListUri);
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

        private string GetRelativeListUri(string relativeUri, int? offset, int? count) {
            NameValueCollection queryStringParameters = HttpUtility.ParseQueryString(string.Empty);
            if (offset.HasValue) {
                queryStringParameters["offset"] = offset.Value.ToString();
            }
            if (count.HasValue) {
                queryStringParameters["count"] = count.Value.ToString();
            }

            var queryString = queryStringParameters.ToString();
            if (!string.IsNullOrEmpty(queryString)) {
                relativeUri = relativeUri + "?" + queryString;
            }

            return relativeUri;
        }
    }
}
