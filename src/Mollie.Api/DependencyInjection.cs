using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Options;
using Polly;

namespace Mollie.Api {
    public static class DependencyInjection {
        public static IServiceCollection AddMollieApi(
            this IServiceCollection services,
            Action<MollieOptions> mollieOptionsDelegate) {

            MollieOptions mollieOptions = new();
            mollieOptionsDelegate.Invoke(mollieOptions);

            if (mollieOptions.CustomMollieSecretManager != null) {
                services.AddScoped(typeof(IMollieSecretManager), mollieOptions.CustomMollieSecretManager);
            }
            else {
                services.AddScoped<IMollieSecretManager, DefaultMollieSecretManager>(_ =>
                    new DefaultMollieSecretManager(mollieOptions.ApiKey));
            }

            MollieClientOptions mollieClientOptions = new() {
                ApiKey = mollieOptions.ApiKey,
                ClientId = mollieOptions.ClientId,
                ClientSecret = mollieOptions.ClientSecret,
                CustomUserAgent = mollieOptions.CustomUserAgent
            };
            services.AddSingleton(mollieClientOptions);

            RegisterMollieApiClient<IBalanceClient, BalanceClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICaptureClient, CaptureClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IChargebackClient, ChargebackClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IConnectClient, ConnectClient>(services,  mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICustomerClient, CustomerClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IInvoiceClient, InvoiceClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IMandateClient, MandateClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOnboardingClient, OnboardingClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrderClient, OrderClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrganizationClient, OrganizationClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentClient, PaymentClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentLinkClient, PaymentLinkClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentMethodClient, PaymentMethodClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPermissionClient, PermissionClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IProfileClient, ProfileClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IRefundClient, RefundClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISettlementClient, SettlementClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IShipmentClient, ShipmentClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISubscriptionClient, SubscriptionClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ITerminalClient, TerminalClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IClientLinkClient, ClientLinkClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IWalletClient, WalletClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IClientClient, ClientClient>(services, mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICapabilityClient, CapabilityClient>(services, mollieOptions.RetryPolicy);

            return services;
        }

        static void RegisterMollieApiClient<TInterface, TImplementation>(
            IServiceCollection services,
            IAsyncPolicy<HttpResponseMessage>? retryPolicy = null)
            where TInterface : class
            where TImplementation : class, TInterface {

            IHttpClientBuilder clientBuilder = services.AddHttpClient<TInterface, TImplementation>();
            if (retryPolicy != null) {
                clientBuilder.AddPolicyHandler(retryPolicy);
            }
        }
    }
}
