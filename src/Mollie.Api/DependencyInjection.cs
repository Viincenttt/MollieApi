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

            services.AddScoped<IMollieSecretManager, DefaultMollieSecretManager>(_ =>
                new DefaultMollieSecretManager(mollieOptions.ApiKey));

            RegisterMollieApiClient<IBalanceClient, BalanceClient>(services, (httpClient, provider) =>
                new BalanceClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICaptureClient, CaptureClient>(services, (httpClient, provider) =>
                new CaptureClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IChargebackClient, ChargebackClient>(services, (httpClient, provider) =>
                new ChargebackClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IConnectClient, ConnectClient>(services, (httpClient, _) =>
                new ConnectClient(mollieOptions.ClientId, mollieOptions.ClientSecret, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICustomerClient, CustomerClient>(services, (httpClient, provider) =>
                new CustomerClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IInvoiceClient, InvoiceClient>(services, (httpClient, provider) =>
                new InvoiceClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IMandateClient, MandateClient>(services, (httpClient, provider) =>
                new MandateClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOnboardingClient, OnboardingClient>(services, (httpClient, provider) =>
                new OnboardingClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrderClient, OrderClient>(services,(httpClient, provider) =>
                new OrderClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrganizationClient, OrganizationClient>(services, (httpClient, provider) =>
                new OrganizationClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentClient, PaymentClient>(services, (httpClient, provider) =>
                new PaymentClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentLinkClient, PaymentLinkClient>(services, (httpClient, provider) =>
                new PaymentLinkClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentMethodClient, PaymentMethodClient>(services, (httpClient, provider) =>
                new PaymentMethodClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPermissionClient, PermissionClient>(services, (httpClient, provider) =>
                new PermissionClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IProfileClient, ProfileClient>(services, (httpClient, provider) =>
                new ProfileClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IRefundClient, RefundClient>(services, (httpClient, provider) =>
                new RefundClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISettlementClient, SettlementClient>(services, (httpClient, provider) =>
                new SettlementClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IShipmentClient, ShipmentClient>(services, (httpClient, provider) =>
                new ShipmentClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISubscriptionClient, SubscriptionClient>(services, (httpClient, provider) =>
                new SubscriptionClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ITerminalClient, TerminalClient>(services, (httpClient, provider) =>
                new TerminalClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IClientLinkClient, ClientLinkClient>(services, (httpClient, provider) =>
                new ClientLinkClient(mollieOptions.ClientId, provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IWalletClient, WalletClient>(services, (httpClient, provider) =>
                new WalletClient(provider.GetRequiredService<IMollieSecretManager>(), httpClient), mollieOptions.RetryPolicy);

            return services;
        }

        static void RegisterMollieApiClient<TInterface, TImplementation>(
            IServiceCollection services,
            Func<HttpClient, IServiceProvider, TImplementation> factory,
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
