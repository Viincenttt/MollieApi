﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class PaymentClient : BaseMollieClient, IPaymentClient {

	    public PaymentClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) { }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest, bool includeQrCode = false) {
            if (!string.IsNullOrWhiteSpace(paymentRequest.ProfileId) || paymentRequest.Testmode.HasValue || paymentRequest.ApplicationFee != null) {
                this.ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = this.BuildQueryParameters(
                includeQrCode: includeQrCode);

            return await this.PostAsync<PaymentResponse>($"payments{queryParameters.ToQueryString()}", paymentRequest).ConfigureAwait(false);
        }

	    public async Task<PaymentResponse> GetPaymentAsync(string paymentId, bool testmode = false, bool includeQrCode = false, bool includeRemainderDetails = false) {
	        if (testmode) {
	            this.ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = this.BuildQueryParameters(
                testmode: testmode, 
                includeQrCode: includeQrCode, 
                includeRemainderDetails: includeRemainderDetails);
			return await this.GetAsync<PaymentResponse>($"payments/{paymentId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
		}

		public async Task DeletePaymentAsync(string paymentId) {
		    await this.DeleteAsync($"payments/{paymentId}").ConfigureAwait(false);
		}

        public async Task<PaymentResponse> GetPaymentAsync(UrlObjectLink<PaymentResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentListAsync(string from = null, int? limit = null, string profileId = null, bool testmode = false, bool includeQrCode = false) {
	        if (!string.IsNullOrWhiteSpace(profileId) || testmode) {
	            this.ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = this.BuildQueryParameters(
                profileId: profileId,
                testmode: testmode,
                includeQrCode: includeQrCode);

            return await this.GetListAsync<ListResponse<PaymentResponse>>($"payments", from, limit, queryParameters).ConfigureAwait(false);
		}

        public async Task<PaymentResponse> UpdatePaymentAsync(string paymentId, PaymentUpdateRequest paymentUpdateRequest) {
            return await this.PatchAsync<PaymentResponse>($"payments/{paymentId}", paymentUpdateRequest).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(string profileId = null, bool testmode = false, bool includeQrCode = false, bool includeRemainderDetails = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue(nameof(testmode), testmode);
            result.AddValueIfNotNullOrEmpty(nameof(profileId), profileId);
            result.AddValueIfNotNullOrEmpty("include", this.BuildIncludeParameter(includeQrCode, includeRemainderDetails));
            return result;
        }

        private string BuildIncludeParameter(bool includeQrCode = false, bool includeRemainderDetails = false) {
            var includeList = new List<string>();
            includeList.AddValueIfTrue("details.qrCode", includeQrCode);
            includeList.AddValueIfTrue("details.remainderDetails", includeRemainderDetails);
            return includeList.ToIncludeParameter();
        }
    }
}