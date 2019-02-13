using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client
{
    public class PaymentMethodClient : BaseMollieClient, IPaymentMethodClient
    {
        public PaymentMethodClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient)
        {
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(PaymentMethod paymentMethod, bool? includeIssuers = null, string locale = null, bool? includePricing = null, string profileId = null, bool? testmode = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.AddValueIfNotNullOrEmpty("locale", locale);
            AddOauthParameters(parameters, profileId, testmode);
            AddIncludeParameters(parameters, includeIssuers, includePricing);

            return await this.GetAsync<PaymentMethodResponse>($"methods/{paymentMethod.ToString().ToLower()}{parameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetAllPaymentMethodListAsync(string locale = null, bool? includeIssuers = null, bool? includePricing = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.AddValueIfNotNullOrEmpty("locale", locale);
            AddIncludeParameters(parameters, includeIssuers, includePricing);

            return await this.GetListAsync<ListResponse<PaymentMethodResponse>>("methods/all", null, null, parameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(SequenceType? sequenceType = null, string locale = null, Amount amount = null, bool? includeIssuers = null, bool? includePricing = null, string profileId = null, bool? testmode = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                {"sequenceType", sequenceType.ToString().ToLower()},
                {"locale", locale},
                {"amount[value]", amount?.Value},
                {"amount[currency]", amount?.Currency}
            };

            AddOauthParameters(parameters, profileId, testmode);
            AddIncludeParameters(parameters, includeIssuers, includePricing);

            return await this.GetListAsync<ListResponse<PaymentMethodResponse>>("methods", null, null, parameters).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(UrlObjectLink<PaymentMethodResponse> url)
        {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        private void AddOauthParameters(Dictionary<string, string> parameters, string profileId = null, bool? testmode = null)
        {
            if (!string.IsNullOrWhiteSpace(profileId) || testmode.HasValue)
            {
                this.ValidateApiKeyIsOauthAccesstoken();

                parameters.AddValueIfNotNullOrEmpty("profileId", profileId);
                if (testmode.HasValue)
                {
                    parameters.AddValueIfNotNullOrEmpty("testmode", testmode.Value.ToString().ToLower());
                }
            }
        }

        private void AddIncludeParameters(Dictionary<string, string> parameters, bool? includeIssuers = null, bool? includePricing = null)
        {
            if (includeIssuers == true)
            {
                parameters.Add("include", "issuers");
            }

            if (includePricing == true)
            {
                parameters.Add("include", "pricing");
            }
        }
    }
}