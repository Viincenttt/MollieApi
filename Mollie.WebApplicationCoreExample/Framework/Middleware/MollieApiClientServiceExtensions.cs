using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;

namespace Mollie.WebApplicationCoreExample.Framework.Middleware {
    public static class MollieApiClientServiceExtensions {
        public static IServiceCollection AddMollieApi(this IServiceCollection services, string apiKey) {
            services.AddScoped<IPaymentClient, PaymentClient>();
            services.AddScoped<IPaymentClient, PaymentClient>(x => new PaymentClient(apiKey));
            services.AddScoped<ICustomerClient, CustomerClient>(x => new CustomerClient(apiKey));
            services.AddScoped<IRefundClient, RefundClient>(x => new RefundClient(apiKey));
            services.AddScoped<IPaymentMethodClient, PaymentMethodClient>(x => new PaymentMethodClient(apiKey));
            services.AddScoped<ISubscriptionClient, SubscriptionClient>(x => new SubscriptionClient(apiKey));
            services.AddScoped<IMandateClient, MandateClient>(x => new MandateClient(apiKey));
            services.AddScoped<IInvoicesClient, InvoicesClient>(x => new InvoicesClient(apiKey));

            return services;
        }
    }
}