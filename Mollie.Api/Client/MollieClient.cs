using System;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Issuer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Subscription;

namespace Mollie.Api.Client {
    
    [Obsolete("This class is deprecated, please use the new API specific clients, such as PaymentClient, CustomerClient etc")]
    public class MollieClient : IMollieClient {
        private readonly IPaymentClient _paymentClient;
        private readonly IPaymentMethodClient _paymentMethodClient;
        private readonly IRefundClient _refundClient;
        private readonly IIssuerClient _issuerClient;
        private readonly ISubscriptionClient _subscriptionClient;
        private readonly IMandateClient _mandateClient;
        private readonly ICustomerClient _customerClient;

        public MollieClient(string apiKey) {
            if (string.IsNullOrEmpty(apiKey)) {
                throw new ArgumentException("Mollie API key cannot be empty");
            }

            this._paymentClient = new PaymentClient(apiKey);
            this._paymentMethodClient = new PaymentMethodClient(apiKey);
            this._refundClient = new RefundClient(apiKey);
            this._issuerClient = new IssuerClient(apiKey);
            this._subscriptionClient = new SubscriptionClient(apiKey);
            this._mandateClient = new MandateClient(apiKey);
            this._customerClient = new CustomerClient(apiKey);
        }

        [Obsolete("This method is deprecated, please use PaymentClient class")]
        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest) {
            return await this._paymentClient.CreatePaymentAsync(paymentRequest);
        }

        [Obsolete("This method is deprecated, please use PaymentClient class")]
        public async Task<ListResponse<PaymentResponse>> GetPaymentListAsync(int? offset = null, int? count = null) {
            return await this._paymentClient.GetPaymentListAsync(offset, count);
        }

        [Obsolete("This method is deprecated, please use PaymentClient class")]
        public async Task<PaymentResponse> GetPaymentAsync(string paymentId) {
            return await this._paymentClient.GetPaymentAsync(paymentId);
        }

        [Obsolete("This method is deprecated, please use PaymentMethodClient class")]
        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(int? offset = null, int? count = null) {
            return await this._paymentMethodClient.GetPaymentMethodListAsync(offset, count);
        }

        [Obsolete("This method is deprecated, please use PaymentMethodClient class")]
        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(PaymentMethod paymentMethod) {
            return await this._paymentMethodClient.GetPaymentMethodAsync(paymentMethod);
        }

        [Obsolete("This method is deprecated, please use IssuerClient class")]
        public async Task<ListResponse<IssuerResponse>> GetIssuerListAsync(int? offset = null, int? count = null) {
            return await this._issuerClient.GetIssuerListAsync(offset, count);
        }

        [Obsolete("This method is deprecated, please use IssuerClient class")]
        public async Task<IssuerResponse> GetIssuerAsync(string issuerId) {
            return await this._issuerClient.GetIssuerAsync(issuerId);
        }

        [Obsolete("This method is deprecated, please use RefundClient class")]
        public async Task<RefundResponse> CreateRefundAsync(string paymentId) {
            return await this._refundClient.CreateRefundAsync(paymentId);
        }

        [Obsolete("This method is deprecated, please use RefundClient class")]
        public async Task<RefundResponse> CreateRefundAsync(string paymentId, RefundRequest refundRequest) {
            return await this._refundClient.CreateRefundAsync(paymentId, refundRequest);
        }

        [Obsolete("This method is deprecated, please use RefundClient class")]
        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(string paymentId, int? offset = null, int? count = null) {
            return await this._refundClient.GetRefundListAsync(paymentId, offset, count);
        }

        [Obsolete("This method is deprecated, please use RefundClient class")]
        public async Task<RefundResponse> GetRefundAsync(string paymentId, string refundId) {
            return await this._refundClient.GetRefundAsync(paymentId, refundId);
        }

        [Obsolete("This method is deprecated, please use RefundClient class")]
        public async Task CancelRefundAsync(string paymentId, string refundId) {
            await this._refundClient.CancelRefundAsync(paymentId, refundId);
        }

        [Obsolete("This method is deprecated, please use CustomerClient class")]
        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request) {
            return await this._customerClient.CreateCustomerAsync(request);
        }

        [Obsolete("This method is deprecated, please use CustomerClient class")]
        public async Task<CustomerResponse> GetCustomerAsync(string customerId) {
            return await this._customerClient.GetCustomerAsync(customerId);
        }

        [Obsolete("This method is deprecated, please use CustomerClient class")]
        public async Task<ListResponse<CustomerResponse>> GetCustomerListAsync(int? offset = null, int? count = null) {
            return await this._customerClient.GetCustomerListAsync(offset, count);
        }

        [Obsolete("This method is deprecated, please use MandateClient class")]
        public async Task<MandateResponse> GetMandateAsync(string customerId, string mandateId) {
            return await this._mandateClient.GetMandateAsync(customerId, mandateId);
        }

        [Obsolete("This method is deprecated, please use MandateClient class")]
        public async Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, int? offset = null, int? count = null) {
            return await this._mandateClient.GetMandateListAsync(customerId, offset, count);
        }

        [Obsolete("This method is deprecated, please use MandateClient class")]
        public async Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request) {
            return await this._mandateClient.CreateMandateAsync(customerId, request);
        }

        [Obsolete("This method is deprecated, please use SubscriptionClient class")]
        public async Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(string customerId, int? offset = null, int? count = null) {
            return await this._subscriptionClient.GetSubscriptionListAsync(customerId, offset, count);
        }

        [Obsolete("This method is deprecated, please use SubscriptionClient class")]
        public async Task<SubscriptionResponse> GetSubscriptionAsync(string customerId, string subscriptionId) {
            return await this._subscriptionClient.GetSubscriptionAsync(customerId, subscriptionId);
        }

        [Obsolete("This method is deprecated, please use SubscriptionClient class")]
        public async Task<SubscriptionResponse> CreateSubscriptionAsync(string customerId, SubscriptionRequest request) {
            return await this._subscriptionClient.CreateSubscriptionAsync(customerId, request);
        }

        [Obsolete("This method is deprecated, please use SubscriptionClient class")]
        public async Task CancelSubscriptionAsync(string customerId, string subscriptionId) {
            await this._subscriptionClient.CancelSubscriptionAsync(customerId, subscriptionId);
        }
    }
}
