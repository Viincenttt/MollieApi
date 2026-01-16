﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client
{
    public class PaymentMethodClient : BaseMollieClient, IPaymentMethodClient
    {
        public PaymentMethodClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public PaymentMethodClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(
            string paymentMethod,
            bool includeIssuers = false,
            string? locale = null,
            string? profileId = null,
            bool testmode = false,
            string? currency = null,
            CancellationToken cancellationToken = default) {

            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);

            Dictionary<string, string> queryParameters = BuildQueryParameters(
                locale: locale,
                currency: currency,
                profileId: profileId,
                testmode: testmode,
                includeIssuers: includeIssuers);

            return await GetAsync<PaymentMethodResponse>(
                $"methods/{paymentMethod.ToLower()}{queryParameters.ToQueryString()}",
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetAllPaymentMethodListAsync(
            string? locale = null,
            Amount? amount = null,
            bool includeIssuers = false,
            bool includePricing = false,
            string? profileId = null,
            bool testmode = false,
            CancellationToken cancellationToken = default) {

            Dictionary<string, string> queryParameters = BuildQueryParameters(
               locale: locale,
               amount: amount,
               includeIssuers: includeIssuers,
               includePricing: includePricing,
               profileId: profileId,
               testmode: testmode);

            return await GetListAsync<ListResponse<PaymentMethodResponse>>(
                "methods/all", null, null, queryParameters,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(
            string? sequenceType = null,
            string? locale = null,
            Amount? amount = null,
            bool includeIssuers = false,
            string? profileId = null,
            bool testmode = false,
            Resource? resource = null,
            string? billingCountry = null,
            string? includeWallets = null,
            CancellationToken cancellationToken = default) {

            Dictionary<string, string> queryParameters = BuildQueryParameters(
               sequenceType: sequenceType,
               locale: locale,
               amount: amount,
               includeIssuers: includeIssuers,
               resource: resource,
               profileId: profileId,
               testmode: testmode,
               billingCountry: billingCountry,
               includeWallets: includeWallets);

            return await GetListAsync<ListResponse<PaymentMethodResponse>>(
                "methods", null, null, queryParameters,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(
            UrlObjectLink<PaymentMethodResponse> url,
            CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(
            string? sequenceType = null,
            string? locale = null,
            Amount? amount = null,
            bool includeIssuers = false,
            bool includePricing = false,
            string? profileId = null,
            bool testmode = false,
            Resource? resource = null,
            string? currency = null,
            string? billingCountry = null,
            string? includeWallets = null) {

            var result = base.BuildQueryParameters(profileId, testmode);
            result.AddValueIfNotNullOrEmpty(nameof(sequenceType), sequenceType?.ToLower());
            result.AddValueIfNotNullOrEmpty(nameof(locale), locale);
            result.AddValueIfNotNullOrEmpty("amount[currency]", amount?.Currency);
            result.AddValueIfNotNullOrEmpty("amount[value]", amount?.Value);
            result.AddValueIfNotNullOrEmpty("include", BuildIncludeParameter(includeIssuers, includePricing));
            result.AddValueIfNotNullOrEmpty(nameof(resource), resource?.ToString()?.ToLower());
            result.AddValueIfNotNullOrEmpty(nameof(currency), currency);
            result.AddValueIfNotNullOrEmpty(nameof(billingCountry), billingCountry);
            result.AddValueIfNotNullOrEmpty(nameof(includeWallets), includeWallets);
            return result;
        }

        private string BuildIncludeParameter(bool includeIssuers = false, bool includePricing = false) {
            var includeList = new List<string>();
            includeList.AddValueIfTrue("issuers", includeIssuers);
            includeList.AddValueIfTrue("pricing", includePricing);
            return includeList.ToIncludeParameter();
        }
    }
}
