﻿using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework;
using Polly;
using Polly.Extensions.Http;

namespace Mollie.WebApplication.Blazor.Framework.Extensions; 

public static class MollieApiClientServiceExtensions {
    public static IServiceCollection AddMollieApi(this IServiceCollection services, IConfiguration configuration) {
        MollieConfiguration mollieConfiguration = configuration.GetSection("Mollie").Get<MollieConfiguration>();
        
        services.AddHttpClient<IPaymentClient, PaymentClient>(httpClient => new PaymentClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<ICustomerClient, CustomerClient>(httpClient => new CustomerClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<IRefundClient, RefundClient>(httpClient => new RefundClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<IPaymentMethodClient, PaymentMethodClient>(httpClient => new PaymentMethodClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<ISubscriptionClient, SubscriptionClient>(httpClient => new SubscriptionClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<IMandateClient, MandateClient>(httpClient => new MandateClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<IInvoicesClient, InvoicesClient>(httpClient => new InvoicesClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<IOrderClient, OrderClient>(httpClient => new OrderClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());
        services.AddHttpClient<ITerminalClient, TerminalClient>(httpClient => new TerminalClient(mollieConfiguration.ApiKey, httpClient))
            .AddPolicyHandler(GetDefaultRetryPolicy());

        return services;
    }
    
    static IAsyncPolicy<HttpResponseMessage> GetDefaultRetryPolicy() {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}