using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Options;
using Polly;

namespace Mollie.Api {
    public static class DependencyInjection {
        public static IServiceCollection AddMollieApi(
            this IServiceCollection services, 
            Action<MollieOptions> mollieOptionsDelegate,
            IAsyncPolicy<HttpResponseMessage> retryPolicy = null) {

            MollieOptions mollieOptions = new MollieOptions();
            mollieOptionsDelegate.Invoke(mollieOptions);

            if (retryPolicy == null && mollieOptions.RetryPolicy != null) {
                retryPolicy = mollieOptions.RetryPolicy;
            }

            RegisterMollieApiClient<IBalanceClient, BalanceClient>(services, httpClient =>
                new BalanceClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<ICaptureClient, CaptureClient>(services, httpClient =>
                new CaptureClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IChargebacksClient, ChargebacksClient>(services, httpClient =>
                new ChargebacksClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IConnectClient, ConnectClient>(services, httpClient =>
                new ConnectClient(mollieOptions.ClientId, mollieOptions.ClientSecret, httpClient), retryPolicy);
            RegisterMollieApiClient<ICustomerClient, CustomerClient>(services, httpClient =>
                new CustomerClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IInvoicesClient, InvoicesClient>(services, httpClient =>
                new InvoicesClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IMandateClient, MandateClient>(services, httpClient =>
                new MandateClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IOnboardingClient, OnboardingClient>(services, httpClient =>
                new OnboardingClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IOrderClient, OrderClient>(services, httpClient =>
                new OrderClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IOrganizationsClient, OrganizationsClient>(services, httpClient =>
                new OrganizationsClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IPaymentClient, PaymentClient>(services, httpClient =>
                new PaymentClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IPaymentLinkClient, PaymentLinkClient>(services, httpClient =>
                new PaymentLinkClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IPaymentMethodClient, PaymentMethodClient>(services, httpClient =>
                new PaymentMethodClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IPermissionsClient, PermissionsClient>(services, httpClient =>
                new PermissionsClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IProfileClient, ProfileClient>(services, httpClient =>
                new ProfileClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IRefundClient, RefundClient>(services, httpClient =>
                new RefundClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<ISettlementsClient, SettlementsClient>(services, httpClient =>
                new SettlementsClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IShipmentClient, ShipmentClient>(services, httpClient =>
                new ShipmentClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<ISubscriptionClient, SubscriptionClient>(services, httpClient =>
                new SubscriptionClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<ITerminalClient, TerminalClient>(services, httpClient =>
                new TerminalClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IClientLinkClient, ClientLinkClient>(services, httpClient =>
                new ClientLinkClient(mollieOptions.ClientId, mollieOptions.ApiKey, httpClient), retryPolicy);
            RegisterMollieApiClient<IWalletClient, WalletClient>(services, httpClient =>
                new WalletClient(mollieOptions.ApiKey, httpClient), retryPolicy);
            
            return services;
        }

        static void RegisterMollieApiClient<TInterface, TImplementation>(
            IServiceCollection services,
            Func<HttpClient, TImplementation> factory,
            IAsyncPolicy<HttpResponseMessage> retryPolicy = null) 
            where TInterface : class
            where TImplementation : class, TInterface {
            
            IHttpClientBuilder clientBuilder = services.AddHttpClient<TInterface, TImplementation>(factory);
            if (retryPolicy != null) {
                clientBuilder.AddPolicyHandler(retryPolicy);
            }
        }
    }
}