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

            IBearerTokenRetriever bearerTokenRetriever = mollieOptions.BearerTokenRetriever ??
                                                         new DefaultBearerTokenRetriever(mollieOptions.ApiKey);

            RegisterMollieApiClient<IBalanceClient, BalanceClient>(services, httpClient =>
                new BalanceClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICaptureClient, CaptureClient>(services, httpClient =>
                new CaptureClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IChargebackClient, ChargebackClient>(services, httpClient =>
                new ChargebackClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IConnectClient, ConnectClient>(services, httpClient =>
                new ConnectClient(mollieOptions.ClientId, mollieOptions.ClientSecret, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ICustomerClient, CustomerClient>(services, httpClient =>
                new CustomerClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IInvoiceClient, InvoiceClient>(services, httpClient =>
                new InvoiceClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IMandateClient, MandateClient>(services, httpClient =>
                new MandateClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOnboardingClient, OnboardingClient>(services, httpClient =>
                new OnboardingClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrderClient, OrderClient>(services, httpClient =>
                new OrderClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IOrganizationClient, OrganizationClient>(services, httpClient =>
                new OrganizationClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentClient, PaymentClient>(services, httpClient =>
                new PaymentClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentLinkClient, PaymentLinkClient>(services, httpClient =>
                new PaymentLinkClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPaymentMethodClient, PaymentMethodClient>(services, httpClient =>
                new PaymentMethodClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IPermissionClient, PermissionClient>(services, httpClient =>
                new PermissionClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IProfileClient, ProfileClient>(services, httpClient =>
                new ProfileClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IRefundClient, RefundClient>(services, httpClient =>
                new RefundClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISettlementClient, SettlementClient>(services, httpClient =>
                new SettlementClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IShipmentClient, ShipmentClient>(services, httpClient =>
                new ShipmentClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ISubscriptionClient, SubscriptionClient>(services, httpClient =>
                new SubscriptionClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<ITerminalClient, TerminalClient>(services, httpClient =>
                new TerminalClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IClientLinkClient, ClientLinkClient>(services, httpClient =>
                new ClientLinkClient(mollieOptions.ClientId, bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);
            RegisterMollieApiClient<IWalletClient, WalletClient>(services, httpClient =>
                new WalletClient(bearerTokenRetriever, httpClient), mollieOptions.RetryPolicy);

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
