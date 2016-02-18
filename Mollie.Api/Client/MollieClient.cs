using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Factories;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Issuer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Refund;
using Newtonsoft.Json;
using RestSharp;

namespace Mollie.Api.Client {
    public class MollieClient {
        public const string ApiEndPoint = "https://api.mollie.nl";
        public const string ApiVersion = "v1";

        private readonly string _apiKey;
        private readonly RestClient _restClient;
        private readonly JsonSerializerSettings _defaultJsonSerializerSettings;

        public MollieClient(string apiKey) {
            this._apiKey = apiKey;
            this._defaultJsonSerializerSettings = this.CreateDefaultJsonSerializerSettings();
            this._restClient = this.CreateRestClient();
        }

        public async Task<PaymentResponse> CreatePayment(PaymentRequest paymentRequest) {
            return await this.Post<PaymentResponse>("payments", paymentRequest);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentList(int? offset = null, int? count = null) {
            return await this.GetList<ListResponse<PaymentResponse>>("payments", offset, count);
        }

        public async Task<PaymentResponse> GetPayment(string paymentId) {
            return await this.Get<PaymentResponse>($"payments/{paymentId}");
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodList(int? offset = null, int? count = null) {
            return await this.GetList<ListResponse<PaymentMethodResponse>>("methods", offset, count);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethod(PaymentMethod paymentMethod) {
            return await this.Get<PaymentMethodResponse>($"methods/{paymentMethod.ToString().ToLower()}");
        }

        public async Task<ListResponse<IssuerResponse>> GetIssuerList(int? offset = null, int? count = null) {
            return await this.GetList<ListResponse<IssuerResponse>>("issuers", offset, count);
        }

        public async Task<IssuerResponse> GetIssuer(string issuerId) {
            return await this.Get<IssuerResponse>($"issuers/{issuerId}");
        }

        public async Task<RefundResponse> CreateRefund(string paymentId, decimal? amount = null) {
            return await this.Post<RefundResponse>($"payments/{paymentId}/refunds", new { amount = amount });
        }

        public async Task<ListResponse<RefundResponse>> GetRefundList(string paymentId, int? offset = null, int? count = null) {
            return await this.GetList<ListResponse<RefundResponse>>($"payments/{paymentId}/refunds", offset, count);
        }

        public async Task<RefundResponse> GetRefund(string paymentId, string refundId) {
            return await this.Get<RefundResponse>($"payments/{paymentId}/refunds/{refundId}");
        }

        public async Task CancelRefund(string paymentId, string refundId) {
            await this.Delete($"payments/{paymentId}/refunds/{refundId}");
        }

        private async Task<T> Get<T>(string relativeUri) {
            RestRequest request = new RestRequest(relativeUri, Method.GET);
            return await this.ExecuteRequest<T>(request);
        }

        private async Task<T> GetList<T>(string relativeUri, int? offset, int? count) {
            RestRequest request = new RestRequest(relativeUri, Method.GET);
            if (offset.HasValue) {
                request.AddParameter("offset", offset);
            }
            if (count.HasValue) {
                request.AddParameter("count", count);
            }

            return await this.ExecuteRequest<T>(request);
        }

        private async Task<T> Post<T>(string relativeUri, object data) {
            RestRequest request = new RestRequest(relativeUri, Method.POST);
            request.AddParameter(String.Empty, JsonConvertExtensions.SerializeObjectCamelCase(data), ParameterType.RequestBody);

            return await this.ExecuteRequest<T>(request);
        }

        private async Task Delete(string relativeUri) {
            RestRequest request = new RestRequest(relativeUri, Method.DELETE);
            await this.ExecuteRequest<object>(request);
        }

        private async Task<T> ExecuteRequest<T>(IRestRequest request) {
            IRestResponse response = await this._restClient.ExecuteTaskAsync(request);
            return this.ProcessHttpResponseMessage<T>(response);
        }

        private T ProcessHttpResponseMessage<T>(IRestResponse response) {
            if (response.IsSuccessful()) {
                return JsonConvert.DeserializeObject<T>(response.Content, this._defaultJsonSerializerSettings);
            }
            else {
                switch (response.StatusCode) {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.MethodNotAllowed:
                    case HttpStatusCode.UnsupportedMediaType:
                    case (HttpStatusCode)422: // Unprocessable entity
                        throw new MollieApiException(response.Content);
                    default:
                        throw new HttpRequestException($"Unknown http exception occured with status code: {(int)response.StatusCode}.");
                }
            }
        }

        /// <summary>
        /// Creates a new rest client for the Mollie API
        /// </summary>
        private RestClient CreateRestClient() {
            RestClient restClient = new RestClient();
            restClient.BaseUrl = this.GetBaseAddress();
            restClient.AddDefaultHeader("Content-Type", "application/json");
            restClient.AddDefaultParameter("Authorization", $"Bearer {this._apiKey}", ParameterType.HttpHeader);

            return restClient;
        }

        /// <summary>
        /// Returns the base address of the Mollie API
        /// </summary>
        /// <returns></returns>
        private Uri GetBaseAddress() => new Uri(ApiEndPoint + "/" + ApiVersion + "/");

        /// <summary>
        /// Creates the default Json serial settings for the JSON.NET parsing.
        /// </summary>
        /// <returns></returns>
        private JsonSerializerSettings CreateDefaultJsonSerializerSettings() {
            return new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>() {
                    // Add a special converter for payment responses, because we need to create specific classes based on the payment method
                    new PaymentResponseConverter(new PaymentResponseFactory())
                }
            };
        }
    }
}
