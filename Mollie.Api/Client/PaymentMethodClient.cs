using System.Collections.Generic;
using System.Linq;
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
        public PaymentMethodClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(string paymentMethod, bool? includeIssuers = null, string locale = null, bool? includePricing = null, string profileId = null, bool? testmode = null, string currency = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.AddValueIfNotNullOrEmpty("locale", locale);
            parameters.AddValueIfNotNullOrEmpty("currency", currency);
            this.AddOauthParameters(parameters, profileId, testmode);
            this.BuildIncludeParameter(parameters, includeIssuers, includePricing);

            return await this.GetAsync<PaymentMethodResponse>($"methods/{paymentMethod.ToString().ToLower()}{parameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetAllPaymentMethodListAsync(string locale = null, bool? includeIssuers = null, bool? includePricing = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.AddValueIfNotNullOrEmpty("locale", locale);
            this.BuildIncludeParameter(parameters, includeIssuers, includePricing);

            return await this.GetListAsync<ListResponse<PaymentMethodResponse>>("methods/all", null, null, parameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(string sequenceType = null, string locale = null, Amount amount = null, bool? includeIssuers = null, bool? includePricing = null, string profileId = null, bool? testmode = null, Resource? resource = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                {"sequenceType", sequenceType.ToString().ToLower()},
                {"locale", locale},
                {"amount[value]", amount?.Value},
                {"amount[currency]", amount?.Currency},
                {"resource", resource.ToString().ToLower()}
            };

            this.AddOauthParameters(parameters, profileId, testmode);
            this.BuildIncludeParameter(parameters, includeIssuers, includePricing);

            return await this.GetListAsync<ListResponse<PaymentMethodResponse>>("methods", null, null, parameters).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(UrlObjectLink<PaymentMethodResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        private void AddOauthParameters(Dictionary<string, string> parameters, string profileId = null, bool? testmode = null) {
            if (!string.IsNullOrWhiteSpace(profileId) || testmode.HasValue) {
                this.ValidateApiKeyIsOauthAccesstoken();

                parameters.AddValueIfNotNullOrEmpty("profileId", profileId);
                if (testmode.HasValue) {
                    parameters.AddValueIfNotNullOrEmpty("testmode", testmode.Value.ToString().ToLower());
                }
            }
        }

        private void BuildIncludeParameter(Dictionary<string, string> parameters, bool? includeIssuers = null, bool? includePricing = null) {
            var includeList = new List<string>();

            if (includeIssuers == true) {
                includeList.Add( "issuers");
            }

            if (includePricing == true) {
                includeList.Add("pricing");
            }

            if (includeList.Any())
            {
                parameters.Add("include", string.Join(",", includeList));
            }
        }
    }
}