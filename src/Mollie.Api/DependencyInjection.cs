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
            Action<MollieOptions> mollieOptionsDelegate) {

            MollieOptions mollieOptions = new MollieOptions();
            mollieOptionsDelegate.Invoke(mollieOptions);

            if (retryPolicy == null) {
                retryPolicy = mollieOptions.RetryPolicy;
            }

            RegisterMollieApiClient<IBalanceClient, BalanceClient>(services, httpClient => 
                new BalanceClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICaptureClient, CaptureClient>(services, httpClient => 
                new CaptureClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IChargebacksClient, ChargebacksClient>(services, httpClient => 
                new ChargebacksClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IConnectClient, ConnectClient>(services, httpClient => 
                new ConnectClient(mollieOptions.ClientId, mollieOptions.ClientSecret, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICustomerClient, CustomerClient>(services, httpClient => 
                new CustomerClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IInvoicesClient, InvoicesClient>(services, httpClient => 
                new InvoicesClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IMandateClient, MandateClient>(services, httpClient => 
                new MandateClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOnboardingClient, OnboardingClient>(services, httpClient => 
                new OnboardingClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrderClient, OrderClient>(services, httpClient => 
                new OrderClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrganizationsClient, OrganizationsClient>(services, httpClient => 
                new OrganizationsClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentClient, PaymentClient>(services, httpClient => 
                new PaymentClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentLinkClient, PaymentLinkClient>(services, httpClient => 
                new PaymentLinkClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentMethodClient, PaymentMethodClient>(services, httpClient => 
                new PaymentMethodClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPermissionsClient, PermissionsClient>(services, httpClient => 
                new PermissionsClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IProfileClient, ProfileClient>(services, httpClient => 
                new ProfileClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IRefundClient, RefundClient>(services, httpClient => 
                new RefundClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISettlementsClient, SettlementsClient>(services, httpClient => 
                new SettlementsClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IShipmentClient, ShipmentClient>(services, httpClient => 
                new ShipmentClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISubscriptionClient, SubscriptionClient>(services, httpClient => 
                new SubscriptionClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ITerminalClient, TerminalClient>(services, httpClient => 
                new TerminalClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IClientLinkClient, ClientLinkClient>(services, httpClient => 
                new ClientLinkClient(mollieOptions.ClientId, mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IWalletClient, WalletClient>(services, httpClient => 
                new WalletClient(mollieOptions.ApiKey, httpClient), mollieOptions.RetryPolicy);
            
            return services;
        }

        static void RegisterMollieApiClient<TInterface, TImplementation>(
            IServiceCollection services,
            Func<HttpClient, TImplementation> factory,
            IAsyncPolicy<HttpResponseMessage>? retryPolicy = null) 
            where TInterface : class
            where TImplementation : class, TInterface {
            
            IHttpClientBuilder clientBuilder = services.AddHttpClient<TInterface, TImplementation>(factory);
            if (retryPolicy != null) {
                clientBuilder.AddPolicyHandler(retryPolicy);
            }
        }
    }
}